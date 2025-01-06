using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Restaurant_Reservation_Client.Models;
using System.Text;

namespace Restaurant_Reservation_Client.Controllers
{
    public class ReservationController : Controller
    {

        private string reservationApi = "https://localhost:7077/api/Reservation/";
        private string arrivalTimeApi = "https://localhost:7077/api/arrivalTime/";
        private HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            List<SeatsViewModel> seats = new List<SeatsViewModel>();
            DateTime todayString = DateTime.Today;
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            HttpResponseMessage response1 = client.GetAsync(reservationApi).Result;
            List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
            HttpResponseMessage response2 = client.GetAsync(arrivalTimeApi).Result;
            if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
            {
                string result1 = response1.Content.ReadAsStringAsync().Result;
                string result2 = response2.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<ReservationViewModel>>(result1);
                var time = JsonConvert.DeserializeObject<List<ArrivalTimeViewModel>>(result2);
                if (data != null && time != null)
                {
                    var modifiedData = data.Where(r => r.BookingDate == todayString).ToList();
                    reservations = modifiedData;
                    arrivalTimes = time;
                    for (var i = 0; i < arrivalTimes.Count; i++)
                    {
                        int requirement = reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                        seats.Add(new SeatsViewModel { Period = arrivalTimes[i].Period, RemainSeats = 45 - requirement });
                    }
                }
            }
            return View(seats);
        }

        [HttpGet]
        public IActionResult MakeRes()
        {
            if (HttpContext.Session.GetString("SelectedDate") != null)
            {
                var selectedDate = HttpContext.Session.GetString("SelectedDate");
                List<ArrivalTimeViewModel> results = new List<ArrivalTimeViewModel>();
                List<ReservationViewModel> reservations = new List<ReservationViewModel>();
                HttpResponseMessage response1 = client.GetAsync(reservationApi).Result;
                List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
                HttpResponseMessage response2 = client.GetAsync(arrivalTimeApi).Result;
                if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                {
                    string result1 = response1.Content.ReadAsStringAsync().Result;
                    string result2 = response2.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<List<ReservationViewModel>>(result1);
                    var time = JsonConvert.DeserializeObject<List<ArrivalTimeViewModel>>(result2);
                    if (data != null && time != null)
                    {
                        var modifiedDate = DateTime.Parse(selectedDate);
                        var modifiedData = data.Where(r => r.BookingDate == modifiedDate).ToList();
                        reservations = modifiedData;
                        arrivalTimes = time;
                        if(modifiedDate == DateTime.Today)
                        {
                            for (var i = 0; i < arrivalTimes.Count; i++)
                            {
                                int requirement = reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                                var diff = Convert.ToInt32((DateTime.Parse(arrivalTimes[i].Period[0..5]) - DateTime.Now).TotalHours);
                                if (requirement < 45 && diff > 1)
                                    results.Add(arrivalTimes[i]);
                            }
                        }
                        else
                        {
                            for (var i = 0; i < arrivalTimes.Count; i++)
                            {
                                int requirement = reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                                if (requirement < 45)
                                    results.Add(arrivalTimes[i]);
                            }
                        }
                    }
                }
                ViewBag.Periods = new SelectList(results, "Id", "Period");
            }
            else
            {
                List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
                HttpResponseMessage getTimesresponse = client.GetAsync(arrivalTimeApi).Result;
                if (getTimesresponse.IsSuccessStatusCode)
                {
                    string getTimesresult = getTimesresponse.Content.ReadAsStringAsync().Result;
                    var timesData = JsonConvert.DeserializeObject<List<ArrivalTimeViewModel>>(getTimesresult);
                    if (timesData != null)
                    {
                        arrivalTimes = timesData;
                    }
                }
                ViewBag.Periods = new SelectList(arrivalTimes, "Id", "Period");
            }
            return View();
        }

        [HttpPost]
        public IActionResult MakeRes(ReservationViewModel reservation)
        {
            List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
            if (HttpContext.Session.GetString("SelectedDate") != null)
            {
                var selectedDate = HttpContext.Session.GetString("SelectedDate");
                List<ArrivalTimeViewModel> results = new List<ArrivalTimeViewModel>();
                List<ReservationViewModel> reservations = new List<ReservationViewModel>();
                HttpResponseMessage response1 = client.GetAsync(reservationApi).Result;
                HttpResponseMessage response2 = client.GetAsync(arrivalTimeApi).Result;
                if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                {
                    string result1 = response1.Content.ReadAsStringAsync().Result;
                    string result2 = response2.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<List<ReservationViewModel>>(result1);
                    var time = JsonConvert.DeserializeObject<List<ArrivalTimeViewModel>>(result2);
                    if (data != null && time != null)
                    {
                        var modifiedDate = DateTime.Parse(selectedDate);
                        var modifiedData = data.Where(r => r.BookingDate == modifiedDate).ToList();
                        reservations = modifiedData;
                        arrivalTimes = time;
                        if (modifiedDate == DateTime.Today)
                        {
                            for (var i = 0; i < arrivalTimes.Count; i++)
                            {
                                int requirement = reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                                var diff = Convert.ToInt32((DateTime.Parse(arrivalTimes[i].Period[0..5]) - DateTime.Now).TotalHours);
                                if (requirement < 45 && diff > 1)
                                    results.Add(arrivalTimes[i]);
                            }
                        }
                        else
                        {
                            for (var i = 0; i < arrivalTimes.Count; i++)
                            {
                                int requirement = reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                                if (requirement < 45)
                                    results.Add(arrivalTimes[i]);
                            }
                        }
                    }
                }
                ViewBag.Periods = new SelectList(results, "Id", "Period");
            }
            else
            {
                HttpResponseMessage getTimesresponse = client.GetAsync(arrivalTimeApi).Result;
                if (getTimesresponse.IsSuccessStatusCode)
                {
                    string getTimesresult = getTimesresponse.Content.ReadAsStringAsync().Result;
                    var timesData = JsonConvert.DeserializeObject<List<ArrivalTimeViewModel>>(getTimesresult);
                    if (timesData != null)
                    {
                        arrivalTimes = timesData;
                    }
                }
                ViewBag.Periods = new SelectList(arrivalTimes, "Id", "Period");
            }
            if (reservation.SeatRequirement <= reservation.ChildSeat)
            {
                ViewBag.ChildSeatError = "兒童座椅數必須少於總訂位數!";
                return View(reservation);
            }
            else if (ModelState.IsValid)
            {
                var selectedTime = arrivalTimes.Where(t => t.Id == reservation.arrivalTimeId).Select(p => p.Period).ToList()[0];
                HttpContext.Session.SetString("selectedTime", selectedTime);
                string data = JsonConvert.SerializeObject(reservation);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage createResponse = client.PostAsync(reservationApi, content).Result;
                if (createResponse.IsSuccessStatusCode)
                {
                    string result = createResponse.Content.ReadAsStringAsync().Result;
                    var detail = JsonConvert.DeserializeObject<ReservationViewModel>(result);
                    int id = detail.Id;
                    HttpContext.Session.SetInt32("ReservationCreator", id);
                    return RedirectToAction("Success");
                }
            }
            return View(reservation);
        }

        [HttpGet]
        public IActionResult Success()
        {
            ReservationViewModel reservation = new();
            var id = HttpContext.Session.GetInt32("ReservationCreator");
            HttpResponseMessage response = client.GetAsync(reservationApi + id).Result;
            if(response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<ReservationViewModel>(result);
                if (data != null)
                {
                    reservation = data;
                }
            }
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
            HttpResponseMessage response = client.GetAsync(reservationApi + "FindByDateAndPhone?bookingDate=" + bookingDate + "&phone=" + phone).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<ReservationViewModel>(result);
                if (data != null)
                {
                    reservation = data;
                    HttpResponseMessage timeResponse = client.GetAsync(arrivalTimeApi + reservation.arrivalTimeId).Result;
                    if (timeResponse.IsSuccessStatusCode)
                    {
                        string getTimeResult = timeResponse.Content.ReadAsStringAsync().Result;
                        var timeData = JsonConvert.DeserializeObject<ArrivalTimeViewModel>(result);
                        if (timeData != null)
                        {
                            ViewBag.Period = timeData;
                        }
                    }
                }
            }
            return View(reservation);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ReservationViewModel reservation = new();
            HttpResponseMessage response = client.GetAsync(reservationApi + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<ReservationViewModel>(result);
                if (data != null)
                {
                    reservation = data;
                    HttpContext.Session.SetInt32("ReservationCreator", id);
                    var selectedDate = reservation.BookingDate;
                    List<ArrivalTimeViewModel> results = new List<ArrivalTimeViewModel>();
                    List<ReservationViewModel> reservations = new List<ReservationViewModel>();
                    HttpResponseMessage response1 = client.GetAsync(reservationApi).Result;
                    List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
                    HttpResponseMessage response2 = client.GetAsync(arrivalTimeApi).Result;
                    if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                    {
                        string result1 = response1.Content.ReadAsStringAsync().Result;
                        string result2 = response2.Content.ReadAsStringAsync().Result;
                        var listOfReservations = JsonConvert.DeserializeObject<List<ReservationViewModel>>(result1);
                        var time = JsonConvert.DeserializeObject<List<ArrivalTimeViewModel>>(result2);
                        if (listOfReservations != null && time != null)
                        {
                            var modifiedData = listOfReservations.Where(r => r.BookingDate == selectedDate).ToList();
                            reservations = modifiedData;
                            arrivalTimes = time;
                            if (selectedDate == DateTime.Today)
                            {
                                for (var i = 0; i < arrivalTimes.Count; i++)
                                {
                                    int requirement = reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                                    var diff = Convert.ToInt32((DateTime.Parse(arrivalTimes[i].Period[0..5]) - DateTime.Now).TotalHours);
                                    if (requirement < 45 && diff > 1)
                                        results.Add(arrivalTimes[i]);
                                }
                            }
                            else
                            {
                                for (var i = 0; i < arrivalTimes.Count; i++)
                                {
                                    int requirement = reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                                    if (requirement < 45)
                                        results.Add(arrivalTimes[i]);
                                }
                            }
                        }
                    }
                    ViewBag.Periods = new SelectList(results, "Id", "Period");
                }
            }
            return View(reservation);
        }

        [HttpPost]
        public IActionResult Edit(ReservationViewModel reservation)
        {
            if (reservation.SeatRequirement <= reservation.ChildSeat)
            {
                ViewBag.ChildSeatError = "兒童座椅數必須少於總訂位數!";
            }
            else
            {
                reservation.Id = HttpContext.Session.GetInt32("ReservationCreator").Value;
                string data = JsonConvert.SerializeObject(reservation);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync(reservationApi + reservation.Id, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Success");
                }
            }
            List<ArrivalTimeViewModel> arrivalTimes = new List<ArrivalTimeViewModel>();
            var selectedDate = reservation.BookingDate;
            List<ArrivalTimeViewModel> results = new List<ArrivalTimeViewModel>();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            HttpResponseMessage response1 = client.GetAsync(reservationApi).Result;
            HttpResponseMessage response2 = client.GetAsync(arrivalTimeApi).Result;
            if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
            {
                string result1 = response1.Content.ReadAsStringAsync().Result;
                string result2 = response2.Content.ReadAsStringAsync().Result;
                var listOfReservation = JsonConvert.DeserializeObject<List<ReservationViewModel>>(result1);
                var time = JsonConvert.DeserializeObject<List<ArrivalTimeViewModel>>(result2);
                if (listOfReservation != null && time != null)
                {
                    var modifiedData = listOfReservation.Where(r => r.BookingDate == selectedDate).ToList();
                    reservations = modifiedData;
                    arrivalTimes = time;
                    if (selectedDate == DateTime.Today)
                    {
                        for (var i = 0; i < arrivalTimes.Count; i++)
                        {
                            int requirement = reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                            var diff = Convert.ToInt32((DateTime.Parse(arrivalTimes[i].Period[0..5]) - DateTime.Now).TotalHours);
                            if (requirement < 45 && diff > 1)
                                results.Add(arrivalTimes[i]);
                        }
                    }
                    else
                    {
                        for (var i = 0; i < arrivalTimes.Count; i++)
                        {
                            int requirement = reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                            if (requirement < 45)
                                results.Add(arrivalTimes[i]);
                        }
                    }
                }
            }
            ViewBag.Periods = new SelectList(results, "Id", "Period");
            return View(reservation);
        }

        public IActionResult Delete(int id)
        {
            HttpResponseMessage response = client.DeleteAsync(reservationApi + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["DeleteMsg"] = "訂位已刪除...下次要再來!!!";
                HttpContext.Session.Remove("ReservationCreator");
                return RedirectToAction("Index");
            }
            return BadRequest();
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
