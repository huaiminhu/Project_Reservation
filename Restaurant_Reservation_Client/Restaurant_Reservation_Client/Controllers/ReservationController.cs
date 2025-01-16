using Microsoft.AspNetCore.Mvc;
using Restaurant_Reservation_Client.Models;
using Restaurant_Reservation_Client.Modules.IServices;
using System.Text;

namespace Restaurant_Reservation_Client.Controllers
{
    public class ReservationController : Controller
    {
        // 使用分層服務
        // 串接訂位資訊API
        private readonly IReservationApiConsuming reservationApiConsuming;
        // 串接訂位時段API
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
            List<ReservationViewModel> reservations = [];
            reservations = await reservationApiConsuming.AllReservations();

            // 取得所有訂位時段
            List<ArrivalTimeViewModel> arrivalTimes = [];
            arrivalTimes = await arrivalTimeApiConsuming.AllArrivalTimes();

            // 將今日時段以及即時空位數新增至seat串列並回傳給檢視
            List<SeatsViewModel> seats = [];
            reservations = reservations.Where(r => r.BookingDate == DateTime.Today).ToList();
            for (var i = 0; i < arrivalTimes.Count; i++)
            {
                int requirement = reservations.Where(r => 
                r.ArrivalTimeId == arrivalTimes[i].Id)
                    .Sum(s => s.SeatRequirement);
                seats.Add(new SeatsViewModel { Period = arrivalTimes[i].Period, 
                    RemainSeats = 45 - requirement });
            }
            return View(seats);
        }

        [HttpGet]   // 新增訂位
        public async Task<IActionResult> MakeRes()
        {
            // 傳送今日開放時段至檢視中時段下拉式選單
            var todayPeriods = await ShowPeriods(DateTime.Today);
            ViewBag.Periods = todayPeriods.ToList();
            
            return View();
        }

        [HttpPost]   // 新增訂位
        public async Task<IActionResult> MakeRes(ReservationViewModel reservation)
        {
            // 傳送今日開放時段至檢視中時段下拉式選單
            var todayPeriods = await ShowPeriods(DateTime.Today);
            ViewBag.Periods = todayPeriods.ToList();

            // 單日重複訂位驗證
            List<ReservationViewModel> reservations = [];
            reservations = await reservationApiConsuming.AllReservations();
            var repeatCheck = reservations.FirstOrDefault(r => 
            r.BookingDate == reservation.BookingDate && 
            r.Phone == reservation.Phone);

            // 訂位數超過剩餘空位數驗證
            int remainSeat = 45 - reservations.Where(r => 
            r.BookingDate == reservation.BookingDate && 
            r.ArrivalTimeId == reservation.ArrivalTimeId)
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
                var selectedTime = await arrivalTimeApiConsuming.ArrivalTimeById(reservation.ArrivalTimeId);
                if(selectedTime!=null)
                    HttpContext.Session.SetString("selectedTime", selectedTime.Period);

                // 使用訂位ID產生SESSION整數供Success在檢視中顯示資訊
                int Reservationid = await reservationApiConsuming.Create(reservation);
                HttpContext.Session.SetInt32("ReservationCreator", Reservationid);
                
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
            int reservationId = HttpContext.Session.GetInt32("ReservationCreator").Value;
            reservation = await reservationApiConsuming.FindReservation(reservationId);
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
            string newDate = bookingDate.ToString("yyyy-MM-dd");
            ReservationViewModel? reservation = await reservationApiConsuming.ResByDateAndPhone(newDate, phone);
            if (reservation != null)
            {
                // 使用訂位ID設定SESSION, 供修改頁面以及訂位完成檢視顯示資訊使用
                HttpContext.Session.SetInt32("ReservationCreator", reservation.Id);

                // 使用時段ID讀取訂位時段, 用於檢視中呈現
                var timeData = await arrivalTimeApiConsuming.ArrivalTimeById(reservation.ArrivalTimeId);

                // 若該時段已過, 讓檢視不再呈現該訂位資訊
                // (檢視中SeatRequirement, 座位需求若為0, 將呈現錯誤訊息)
                if (timeData != null)
                {
                    var diff = Convert.ToInt32((DateTime.Parse(timeData.Period[0..5]) - DateTime.Now).TotalHours);
                    if (diff < 1 && reservation.BookingDate == DateTime.Today)
                        reservation.SeatRequirement = 0;
                    ViewBag.Period = timeData.Period;
                }
            }
            else
            {
                // 找不到訂位資訊
                reservation = new ReservationViewModel() { SeatRequirement = 0 };
            }
            return View(reservation);
        }

        [HttpGet]   // 修改訂位資訊
        public async Task<IActionResult> Edit(int reservationId)
        {
            // 讀取原訂位資訊
            if (HttpContext.Session.GetInt32("ReservationCreator") != null)
                reservationId = HttpContext.Session.GetInt32("ReservationCreator").Value;
            ReservationViewModel? reservation = await reservationApiConsuming.FindReservation(reservationId);

            // 傳送今日開放時段至檢視中時段下拉式選單
            if (reservation != null)
            {
                var todayPeriods = await ShowPeriods(reservation.BookingDate);
                ViewBag.Periods = todayPeriods.ToList();
            }
            
            return View(reservation);
        }

        [HttpPost]   // 修改訂位資訊
        public async Task<IActionResult> Edit(ReservationViewModel reservation)
        {
            if (reservation != null)
            {
                // 使用SESSION設定訂位資訊ID供資料傳遞給後端更新
                reservation.Id = HttpContext.Session.GetInt32("ReservationCreator").Value;

                // 傳送今日開放時段至檢視中時段下拉式選單
                var todayPeriods = await ShowPeriods(reservation.BookingDate);
                ViewBag.Periods = todayPeriods.ToList();

                // 訂位數超過剩餘空位數驗證
                List<ReservationViewModel> reservations = [];
                reservations = await reservationApiConsuming.AllReservations();
                int remainSeat = 45 - reservations.Where(r =>
                r.BookingDate == reservation.BookingDate &&
                r.ArrivalTimeId == reservation.ArrivalTimeId)
                .Sum(s => s.SeatRequirement);
                int remainSeatCheck = remainSeat - reservation.SeatRequirement;

                // 通過所有驗證即更新訂位資訊
                if (reservation.SeatRequirement <= reservation.ChildSeat)
                    ViewBag.ChildSeatError = "兒童座椅數必須少於總訂位數!";
                else if (remainSeatCheck < 0)
                    ViewBag.RemainSeatError = "剩餘座位數不夠!換個時段吧~";
                else
                {                    
                    // 設定SESSION傳遞訂位時段資訊供成功頁面檢視呈現
                    var selectedTime = await arrivalTimeApiConsuming.ArrivalTimeById(reservation.ArrivalTimeId);
                    if (selectedTime != null)
                        HttpContext.Session.SetString("selectedTime", selectedTime.Period);
                    
                    // 更新訂位資訊, 更新後導向訂位成功頁面呈現訂位資訊
                    await reservationApiConsuming.Update(reservation);
                    return RedirectToAction("Success");
                }
            }
            return View(reservation);
        }

        // 取消訂位
        public async Task<IActionResult> Delete(int id)
        {
            // 串接API SERVER執行取消
            await reservationApiConsuming.Delete(id);
            
            // 首頁檢視中Alert呈現取消訂位訊息
            TempData["DeleteMsg"] = "訂位已取消...下次要再來!!!";

            // 移除訂位時產生的SESSION
            HttpContext.Session.Remove("ReservationCreator");

            // 取消成功將返回首頁
            return RedirectToAction("Index");
        }

        // 可選時段供檢視下拉式選單呈現
        public async Task<List<DisplayViewModel>> ShowPeriods(DateTime bookingDate)
        {
            List<DisplayViewModel> results = [];
            List<ReservationViewModel> reservations = await reservationApiConsuming.AllReservations();
            List<ArrivalTimeViewModel> arrivalTimes = await arrivalTimeApiConsuming.AllArrivalTimes();
            results = showPeriods.ListOfPeriods(bookingDate, results, reservations, arrivalTimes);
            return results;
        }
    }
}