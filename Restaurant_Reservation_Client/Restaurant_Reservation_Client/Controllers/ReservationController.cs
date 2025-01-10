using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant_Reservation_Client.Models;
using Restaurant_Reservation_Client.Modules.IServices;
using System.Text;

namespace Restaurant_Reservation_Client.Controllers
{
    public class ReservationController : Controller
    {
        // API串接用端點
        private string reservationApi = "https://localhost:7077/api/Reservation/";
        private string arrivalTimeApi = "https://localhost:7077/api/ArrivalTime/";
        
        private HttpClient client = new HttpClient();

        // 使用分層服務
        private readonly IReservationApiConsuming reservationApiConsuming;
        private readonly IArrivalTimeApiConsuming arrivalTimeApiConsuming;
        private readonly IShowPeriods showPeriods;   // 顯示可選訂位時段

        public ReservationController(IReservationApiConsuming reservationApiConsuming, 
            IArrivalTimeApiConsuming arrivalTimeApiConsuming,
            IShowPeriods showPeriods)
        {
            this.reservationApiConsuming = reservationApiConsuming;
            this.arrivalTimeApiConsuming = arrivalTimeApiConsuming;
            this.showPeriods = showPeriods;
        }

        [HttpGet]   // 家庭餐廳訂位首頁
        public async Task<IActionResult> Index()
        {
            // 取得所有訂位資訊
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            reservations = await reservationApiConsuming.AllReservations(client, reservationApi);

            // 取得所有訂位時段
            List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
            arrivalTimes = await arrivalTimeApiConsuming.AllArrivalTimes(client, arrivalTimeApi);

            // 將今日時段以及即時空位數新增至seat串列並回傳給檢視
            List<SeatsViewModel> seats = new List<SeatsViewModel>();
            reservations = reservations.Where(r => r.BookingDate == DateTime.Today).ToList();
            for (var i = 0; i < arrivalTimes.Count; i++)
            {
                int requirement = reservations.Where(r => 
                r.arrivalTimeId == arrivalTimes[i].Id)
                    .Sum(s => s.SeatRequirement);
                seats.Add(new SeatsViewModel { Period = arrivalTimes[i].Period, 
                    RemainSeats = 45 - requirement });
            }
            return View(seats);
        }

        [HttpGet]   // 新增訂位
        public async Task<IActionResult> MakeRes()
        {
            List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
            arrivalTimes = await arrivalTimeApiConsuming.AllArrivalTimes(client, arrivalTimeApi);
            
            // 若有選定日期, 選定後將產生日期SESSION字串, 再由MakeRes方法讀取 
            if (HttpContext.Session.GetString("SelectedDate") != null)
            {
                var selectedDate = HttpContext.Session.GetString("SelectedDate");
                List<DisplayViewModel> results = new List<DisplayViewModel>();
                List<ReservationViewModel> reservations = new List<ReservationViewModel>();
                reservations = await reservationApiConsuming.AllReservations(client, reservationApi);
                var parsedDate = DateTime.Parse(selectedDate);

                // 依據選定日期, 在檢視當中時段下拉式選單呈現可選時段以及即時空位數
                results = showPeriods.ListOfPeriods(parsedDate, results, reservations, arrivalTimes);
                ViewBag.Periods = new SelectList(results, "Id", "Display");
            }
            else
            {
                ViewBag.Periods = new SelectList(arrivalTimes, "Id", "Period");
            }
            return View();
        }

        [HttpPost]   // 新增訂位
        public async Task<IActionResult> MakeRes(ReservationViewModel reservation)
        {
            // 呈現檢視中下拉式選單資料
            List<DisplayViewModel> results = new List<DisplayViewModel>();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
            arrivalTimes = await arrivalTimeApiConsuming.AllArrivalTimes(client, arrivalTimeApi);
            if (HttpContext.Session.GetString("SelectedDate") != null)
            {
                var selectedDate = HttpContext.Session.GetString("SelectedDate");
                reservations = await reservationApiConsuming.AllReservations(client, reservationApi);
                var parsedDate = DateTime.Parse(selectedDate);
                results = showPeriods.ListOfPeriods(parsedDate, results, reservations, arrivalTimes);
                ViewBag.Periods = new SelectList(results, "Id", "Display");
            }
            else
            {
                ViewBag.Periods = new SelectList(arrivalTimes, "Id", "Period");
            }

            // 單日重複訂位驗證
            var repeatCheck = reservations.FirstOrDefault(r => 
            r.BookingDate == reservation.BookingDate && 
            r.Phone == reservation.Phone);

            // 訂位數超過剩餘空位數驗證
            int remainSeat = 45 - reservations.Where(r => 
            r.BookingDate == reservation.BookingDate && 
            r.arrivalTimeId == reservation.arrivalTimeId)
                .Sum(s => s.SeatRequirement);
            int remainSeatCheck = remainSeat - reservation.SeatRequirement;
            
            // 通過所有驗證即完成訂位
            if (reservation.SeatRequirement <= reservation.ChildSeat)
                ViewBag.ChildSeatError = "兒童座椅數必須少於總訂位數!";
            else if (remainSeatCheck < 0)
                ViewBag.RemainSeatError = "剩餘座位數不夠!換個時段吧~";
            else if (repeatCheck !=null)
                ViewBag.RepeatError = "當日最多訂位一次!";
            else if (ModelState.IsValid)
            {
                // 產生所選時段SESSION字串供Success(成功訂位頁面)在檢視中顯示資訊
                var selectedTime = results.Where(t => t.Id == reservation.arrivalTimeId)
                    .Select(p => p.Display[0..13]).ToList()[0];
                HttpContext.Session.SetString("selectedTime", selectedTime);

                // 使用訂位ID產生SESSION整數供Success在檢視中顯示資訊
                int Reservationid = await reservationApiConsuming.Create(reservation, client, reservationApi);
                HttpContext.Session.SetInt32("ReservationCreator", Reservationid);
                
                // 移除不再使用的SESSION
                HttpContext.Session.Remove("SelectedDate");

                // 完成訂位將導至Success(成功訂位頁面)
                return RedirectToAction("Success");
            }
            return View(reservation);
        }

        [HttpGet]   // 成功訂位
        public async Task<IActionResult> Success()
        {
            // 取得訂位資訊並顯示於檢視
            ReservationViewModel? reservation = new ReservationViewModel();
            var reservationId = HttpContext.Session.GetInt32("ReservationCreator").Value;
            reservation = await reservationApiConsuming.FindReservation(reservationId, client, reservationApi);
            ViewBag.Period = HttpContext.Session.GetString("selectedTime");
            return View(reservation);
        }

        [HttpGet]   // 訂位查詢
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]   // 訂位查詢(輸入日期及連絡電話查詢)
        public async Task<IActionResult> Search(DateTime bookingDate, string phone)
        {
            // 讀取訂位資訊
            ReservationViewModel? reservation = new ReservationViewModel();
            string newDate = bookingDate.ToString("yyyy-MM-dd");
            reservation = await reservationApiConsuming.ResByDateAndPhone(newDate, phone, client, reservationApi);
            HttpContext.Session.SetInt32("ReservationCreator", reservation.Id);
            
            // 使用時段ID讀取訂位時段, 用於檢視中呈現
            var timeData = arrivalTimeApiConsuming.ArrivalTimeById(reservation.arrivalTimeId, client, arrivalTimeApi);

            // 若該時段已過, 讓檢視不再呈現該訂位資訊
            // (檢視中SeatRequirement, 座位需求若為0, 將不呈現訂位資訊)
            var diff = Convert.ToInt32((DateTime.Parse(timeData.Result.Period[0..5]) - DateTime.Now).TotalHours);
            if (diff < 1 && reservation.BookingDate == DateTime.Today)
                reservation.SeatRequirement = 0;

            ViewBag.Period = timeData.Result.Period;
            HttpContext.Session.Remove("DateForQuery");
            return View(reservation);
        }

        [HttpGet]   // 修改訂位資訊
        public async Task<IActionResult> Edit(int reservationId)
        {
            // 讀取原訂位資訊
            ReservationViewModel reservation = new ReservationViewModel();
            if(HttpContext.Session.GetInt32("ReservationCreator") !=null)
                reservationId = HttpContext.Session.GetInt32("ReservationCreator").Value;
            reservation = await reservationApiConsuming.FindReservation(reservationId, client, reservationApi);
            
            // 在檢視中更改日期
            if (HttpContext.Session.GetString("ChangedDate") != null)
                reservation.BookingDate = DateTime.Parse(HttpContext.Session.GetString("ChangedDate"));
            
            // 呈現檢視中下拉式選單可選時段
            List<DisplayViewModel> results = new List<DisplayViewModel>();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
            reservations = await reservationApiConsuming.AllReservations(client, reservationApi);
            arrivalTimes = await arrivalTimeApiConsuming.AllArrivalTimes(client, arrivalTimeApi);
            results = showPeriods.ListOfPeriods(reservation.BookingDate, results, reservations, arrivalTimes);
            ViewBag.Periods = new SelectList(results, "Id", "Display");
            return View(reservation);
        }

        [HttpPost]   // 修改訂位資訊
        public async Task<IActionResult> Edit(ReservationViewModel reservation)
        {
            // 呈現檢視中下拉式選單可選時段
            List<DisplayViewModel> results = new List<DisplayViewModel>();
            if (HttpContext.Session.GetString("ChangedDate") != null)
                reservation.BookingDate = DateTime.Parse(HttpContext.Session.GetString("ChangedDate"));
            List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            reservations = await reservationApiConsuming.AllReservations(client, reservationApi);
            arrivalTimes = await arrivalTimeApiConsuming.AllArrivalTimes(client, arrivalTimeApi);
            results = showPeriods.ListOfPeriods(reservation.BookingDate, results, reservations, arrivalTimes);
            ViewBag.Periods = new SelectList(results, "Id", "Display");

            // 訂位數超過剩餘空位數驗證
            int remainSeat = 45 - reservations.Where(r => 
            r.BookingDate == reservation.BookingDate && 
            r.arrivalTimeId == reservation.arrivalTimeId)
                .Sum(s => s.SeatRequirement);
            int remainSeatCheck = remainSeat - reservation.SeatRequirement;

            // 通過所有驗證即更新訂位資訊
            if (reservation.SeatRequirement <= reservation.ChildSeat)
                ViewBag.ChildSeatError = "兒童座椅數必須少於總訂位數!";
            else if (remainSeatCheck < 0)
                ViewBag.RemainSeatError = "剩餘座位數不夠!換個時段吧~";
            else
            {
                // 更新訂位資訊, 更新後導向訂位成功頁面呈現訂位資訊
                var selectedTime = results.Where(t => 
                t.Id == reservation.arrivalTimeId)
                    .Select(p => p.Display[0..13]).ToList()[0];
                reservation.Id = HttpContext.Session.GetInt32("ReservationCreator").Value;
                await reservationApiConsuming.Update(reservation, client, reservationApi);
                HttpContext.Session.SetString("selectedTime", selectedTime);
                HttpContext.Session.Remove("ChangedDate");
                return RedirectToAction("Success");
            }
            return View(reservation);
        }

        // 取消訂位
        public async Task<IActionResult> Delete(int id)
        {
            await reservationApiConsuming.Delete(id, client, reservationApi);
            
            // 首頁檢視中Alert呈現取消訂位訊息
            TempData["DeleteMsg"] = "訂位已取消...下次要再來!!!";

            // 移除訂位時產生的SESSION
            HttpContext.Session.Remove("ReservationCreator");

            // 取消成功將返回首頁
            return RedirectToAction("Index");
        }

        // 在檢視中(MakeRes, 訂位頁面)的MODAL選定日期後,
        // 產生日期SESSION字串
        [HttpPost]   
        public IActionResult SelectDate(DateTime bookingDate)
        {
            HttpContext.Session.SetString("SelectedDate", bookingDate.ToString());
            return RedirectToAction("MakeRes");
        }


        // 在檢視中(Edit, 修改訂位資訊)的MODAL選定日期後,
        // 產生日期SESSION字串
        [HttpPost]   
        public IActionResult ChangeDate(DateTime bookingDate)
        {
            HttpContext.Session.SetString("ChangedDate", bookingDate.ToString());
            return RedirectToAction("Edit");
        }


        // 在檢視中(Search, 查詢訂位頁面)的MODAL選定日期後,
        // 產生日期SESSION字串
        [HttpPost]   
        public IActionResult DateForQuery(DateTime bookingDate)
        {
            HttpContext.Session.SetString("DateForQuery", bookingDate.ToString());
            return RedirectToAction("Search");
        }
    }
}
