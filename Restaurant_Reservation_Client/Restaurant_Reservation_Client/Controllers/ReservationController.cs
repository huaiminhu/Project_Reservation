using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant_Reservation_Client.Models;
using Restaurant_Reservation_Client.Modules.IServices;
using System.Text;

namespace Restaurant_Reservation_Client.Controllers
{
    public class ReservationController : Controller
    {

        private string reservationApi = "https://localhost:7077/api/Reservation/";
        private string arrivalTimeApi = "https://localhost:7077/api/ArrivalTime/";
        private HttpClient client = new HttpClient();

        private readonly IReservationApiConsuming reservationApiConsuming;
        private readonly IArrivalTimeApiConsuming arrivalTimeApiConsuming;
        private readonly IShowPeriods showPeriods;

        public ReservationController(IReservationApiConsuming reservationApiConsuming, 
            IArrivalTimeApiConsuming arrivalTimeApiConsuming,
            IShowPeriods showPeriods)
        {
            this.reservationApiConsuming = reservationApiConsuming;
            this.arrivalTimeApiConsuming = arrivalTimeApiConsuming;
            this.showPeriods = showPeriods;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<SeatsViewModel> seats = new List<SeatsViewModel>();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            reservations = reservationApiConsuming.AllReservations(client, reservationApi);
            List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
            arrivalTimes = arrivalTimeApiConsuming.AllArrivalTimes(client, arrivalTimeApi);
            reservations = reservations.Where(r => r.BookingDate == DateTime.Today).ToList();
            for (var i = 0; i < arrivalTimes.Count; i++)
            {
                int requirement = reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                seats.Add(new SeatsViewModel { Period = arrivalTimes[i].Period, RemainSeats = 45 - requirement });
            }
            return View(seats);
        }

        [HttpGet]
        public IActionResult MakeRes()
        {
            List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
            if (HttpContext.Session.GetString("SelectedDate") != null)
            {
                var selectedDate = HttpContext.Session.GetString("SelectedDate");
                List<DisplayViewModel> results = new List<DisplayViewModel>();
                List<ReservationViewModel> reservations = new List<ReservationViewModel>();
                reservations = reservationApiConsuming.AllReservations(client, reservationApi);
                arrivalTimes = arrivalTimeApiConsuming.AllArrivalTimes(client, arrivalTimeApi);
                var parsedDate = DateTime.Parse(selectedDate);
                results = showPeriods.periods(parsedDate, results, reservations, arrivalTimes);
                ViewBag.Periods = new SelectList(results, "Id", "Display");
            }
            else
            {
                arrivalTimes = arrivalTimeApiConsuming.AllArrivalTimes(client, arrivalTimeApi);
                ViewBag.Periods = new SelectList(arrivalTimes, "Id", "Period");
            }
            return View();
        }

        [HttpPost]
        public IActionResult MakeRes(ReservationViewModel reservation)
        {
            int remainSeat = 0;
            List<DisplayViewModel> results = new List<DisplayViewModel>();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
            if (HttpContext.Session.GetString("SelectedDate") != null)
            {
                var selectedDate = HttpContext.Session.GetString("SelectedDate");
                reservations = reservationApiConsuming.AllReservations(client, reservationApi);
                arrivalTimes = arrivalTimeApiConsuming.AllArrivalTimes(client, arrivalTimeApi);
                var parsedDate = DateTime.Parse(selectedDate);
                results = showPeriods.periods(parsedDate, results, reservations, arrivalTimes);
                ViewBag.Periods = new SelectList(results, "Id", "Display");
            }
            else
            {
                arrivalTimes = arrivalTimeApiConsuming.AllArrivalTimes(client, arrivalTimeApi);
                ViewBag.Periods = new SelectList(arrivalTimes, "Id", "Period");
            }
            var repeatCheck = reservations.FirstOrDefault(r => r.BookingDate == reservation.BookingDate && r.Phone == reservation.Phone);
            remainSeat = 45 - reservations.Where(r => r.BookingDate == reservation.BookingDate && r.arrivalTimeId == reservation.arrivalTimeId).Sum(s => s.SeatRequirement);
            var remainSeatCheck = remainSeat - reservation.SeatRequirement;
            if (reservation.SeatRequirement <= reservation.ChildSeat)
                ViewBag.ChildSeatError = "兒童座椅數必須少於總訂位數!";
            else if (remainSeatCheck < 0)
                ViewBag.RemainSeatError = "剩餘座位數不夠!換個時段吧~";
            else if (repeatCheck !=null)
                ViewBag.RepeatError = "當日最多訂位一次!";
            else if (ModelState.IsValid)
            {
                var selectedTime = results.Where(t => t.Id == reservation.arrivalTimeId).Select(p => p.Display[0..13]).ToList()[0];
                HttpContext.Session.SetString("selectedTime", selectedTime);
                int Reservationid = reservationApiConsuming.Create(reservation, client, reservationApi);
                HttpContext.Session.SetInt32("ReservationCreator", Reservationid);
                HttpContext.Session.Remove("SelectedDate");
                return RedirectToAction("Success");
            }
            return View(reservation);
        }

        [HttpGet]
        public IActionResult Success()
        {
            ReservationViewModel reservation = new();
            var reservationId = HttpContext.Session.GetInt32("ReservationCreator").Value;
            reservation = reservationApiConsuming.FindReservation(reservationId, client, reservationApi);
            ViewBag.Period = HttpContext.Session.GetString("selectedTime");
            return View(reservation);
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(DateTime bookingDate, string phone)
        {
            ReservationViewModel reservation = new();
            string newDate = bookingDate.ToString("yyyy-MM-dd");
            reservation = reservationApiConsuming.ResByDateAndPhone(newDate, phone, client, reservationApi);
            HttpContext.Session.SetInt32("ReservationCreator", reservation.Id);
            var timeData = arrivalTimeApiConsuming.ArrivalTimeById(reservation.arrivalTimeId, client, arrivalTimeApi);
            var diff = Convert.ToInt32((DateTime.Parse(timeData.Period[0..5]) - DateTime.Now).TotalHours);
            if (diff < 1 && reservation.BookingDate == DateTime.Today)
                reservation.SeatRequirement = 0;
            ViewBag.Period = timeData.Period;
            HttpContext.Session.Remove("DateForQuery");
            return View(reservation);
        }

        [HttpGet]
        public IActionResult Edit(int reservationId)
        {
            int remainSeat = 0;
            ReservationViewModel reservation = new();
            if(HttpContext.Session.GetInt32("ReservationCreator") !=null)
                reservationId = HttpContext.Session.GetInt32("ReservationCreator").Value;
            reservation = reservationApiConsuming.FindReservation(reservationId, client, reservationApi);
            if (HttpContext.Session.GetString("ChangedDate") != null)
                reservation.BookingDate = DateTime.Parse(HttpContext.Session.GetString("ChangedDate"));
            List<DisplayViewModel> results = new List<DisplayViewModel>();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
            reservations = reservationApiConsuming.AllReservations(client, reservationApi);
            arrivalTimes = arrivalTimeApiConsuming.AllArrivalTimes(client, arrivalTimeApi);
            results = showPeriods.periods(reservation.BookingDate, results, reservations, arrivalTimes);
            ViewBag.Periods = new SelectList(results, "Id", "Display");
            return View(reservation);
        }

        [HttpPost]
        public IActionResult Edit(ReservationViewModel reservation)
        {
            int remainSeat = 0;
            List<DisplayViewModel> results = new List<DisplayViewModel>();
            if (HttpContext.Session.GetString("ChangedDate") != null)
                reservation.BookingDate = DateTime.Parse(HttpContext.Session.GetString("ChangedDate"));
            List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            reservations = reservationApiConsuming.AllReservations(client, reservationApi);
            arrivalTimes = arrivalTimeApiConsuming.AllArrivalTimes(client, arrivalTimeApi);
            results = showPeriods.periods(reservation.BookingDate, results, reservations, arrivalTimes);
            ViewBag.Periods = new SelectList(results, "Id", "Display");
            remainSeat = 45 - reservations.Where(r => r.BookingDate == reservation.BookingDate && r.arrivalTimeId == reservation.arrivalTimeId).Sum(s => s.SeatRequirement);
            var remainSeatCheck = remainSeat - reservation.SeatRequirement;
            if (reservation.SeatRequirement <= reservation.ChildSeat)
                ViewBag.ChildSeatError = "兒童座椅數必須少於總訂位數!";
            else if (remainSeatCheck < 0)
                ViewBag.RemainSeatError = "剩餘座位數不夠!換個時段吧~";
            else
            {
                var selectedTime = results.Where(t => t.Id == reservation.arrivalTimeId).Select(p => p.Display[0..13]).ToList()[0];
                reservation.Id = HttpContext.Session.GetInt32("ReservationCreator").Value;
                reservationApiConsuming.Update(reservation, client, reservationApi);
                HttpContext.Session.SetString("selectedTime", selectedTime);
                HttpContext.Session.Remove("ChangedDate");
                return RedirectToAction("Success");
            }
            return View(reservation);
        }

        public IActionResult Delete(int id)
        {
            reservationApiConsuming.Delete(id, client, reservationApi);
            TempData["DeleteMsg"] = "訂位已刪除...下次要再來!!!";
            HttpContext.Session.Remove("ReservationCreator");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SelectDate(DateTime bookingDate)
        {
            HttpContext.Session.SetString("SelectedDate", bookingDate.ToString());
            return RedirectToAction("MakeRes");
        }

        [HttpPost]
        public IActionResult ChangeDate(DateTime bookingDate)
        {
            HttpContext.Session.SetString("ChangedDate", bookingDate.ToString());
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public IActionResult DateForQuery(DateTime bookingDate)
        {
            HttpContext.Session.SetString("DateForQuery", bookingDate.ToString());
            return RedirectToAction("Search");
        }
    }
}
