using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Restaurant_Reservation_Client.Models;
using Restaurant_Reservation_Client.Controllers;
using System.Security.Principal;
using System.Text;

namespace Restaurant_Reservation_Client.Controllers
{
    public class ReservationController : Controller
    {

        private string url1 = "https://localhost:7077/api/Reservation/";
        private string url2 = "https://localhost:7077/api/ArrivedTime/";
        private HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            List<SeatsViewModel> seats = new List<SeatsViewModel>();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            HttpResponseMessage response1 = client.GetAsync(url1).Result;
            List<ArrivedTimeViewModel> arrivedTimes = new List<ArrivedTimeViewModel>();
            HttpResponseMessage response2 = client.GetAsync(url2).Result;
            if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
            {
                string result1 = response1.Content.ReadAsStringAsync().Result;
                string result2 = response2.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<ReservationViewModel>>(result1);
                var time = JsonConvert.DeserializeObject<List<ArrivedTimeViewModel>>(result2);
                if (data != null && time != null)
                {
                    reservations = data;
                    arrivedTimes = time;
                    for (var i = 0; i < arrivedTimes.Count; i++)
                    {
                        int requirement = 0;
                        if(reservations.Count != 0) 
                        {
                            for (var j = 0; j <= reservations.Count - 1; j++)
                            {
                                if (reservations[j].BookingDate == DateTime.Today.ToString("d").Replace('/', '-') && arrivedTimes[i].Id == reservations[j].ArrivedTimeId)
                                {
                                    requirement += reservations[j].SeatRequirement;
                                }
                            }
                            seats.Add(new SeatsViewModel { Period = arrivedTimes[i].Period, RemainSeats = 40 - requirement });
                        }
                        else
                            seats.Add(new SeatsViewModel { Period = arrivedTimes[i].Period, RemainSeats = 40 });
                    }
                }
            }
            return View(seats);
        }

        [HttpGet]
        public IActionResult MakeRes()
        {
            List<ArrivedTimeViewModel> arrivedTimes = new List<ArrivedTimeViewModel>();
            HttpResponseMessage response = client.GetAsync(url2).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<ArrivedTimeViewModel>>(result);
                if (data != null)
                {
                    arrivedTimes = data;
                }
            }
            arrivedTimes.Insert(0, new ArrivedTimeViewModel { Id = 0, Period = "請選擇時段" });
            ViewBag.Periods = new SelectList(arrivedTimes, "Id", "Period");
            return View();
        }

        [HttpPost]
        public IActionResult MakeRes(ReservationViewModel reservation)
        {
            HttpResponseMessage response = new();
            if (ModelState.IsValid)
            {
                string data = JsonConvert.SerializeObject(reservation);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                response = client.PostAsync(url1, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    var detail = JsonConvert.DeserializeObject<ReservationViewModel>(result);
                    int id = detail.Id;
                    HttpContext.Session.SetInt32("ReservationCreator", id);
                    return RedirectToAction("Success");
                }
            }
            List<ArrivedTimeViewModel> arrivedTimes = new List<ArrivedTimeViewModel>();
            response = client.GetAsync(url2).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<ArrivedTimeViewModel>>(result);
                if (data != null)
                {
                    arrivedTimes = data;
                }
            }
            arrivedTimes.Insert(0, new ArrivedTimeViewModel { Id = 0, Period = "請選擇時段" });
            ViewBag.Periods = new SelectList(arrivedTimes, "Id", "Period");
            return View(reservation);
        }

        [HttpGet]
        public IActionResult Success()
        {
            ReservationViewModel reservation = new();
            var id = HttpContext.Session.GetInt32("ReservationCreator");
            HttpResponseMessage response = client.GetAsync(url1 + id).Result;
            if(response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<ReservationViewModel>(result);
                if (data != null)
                {
                    reservation = data;
                    response = client.GetAsync(url2 + reservation.ArrivedTimeId).Result;
                    if( response.IsSuccessStatusCode )
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                        var period = JsonConvert.DeserializeObject<ArrivedTimeViewModel>(result);
                        if (period != null)
                        {
                            ViewBag.Period = period.Period;
                        }
                    }
                }
            }
            return View(reservation);
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string phone)
        {
            ReservationViewModel reservation = new ReservationViewModel();
            StringContent content = new StringContent(phone, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url1 + "FindResByPhone?phone=" + phone, content).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<ReservationViewModel>(result);
                if (data != null)
                {
                    reservation = data;
                    response = client.GetAsync(url2 + reservation.ArrivedTimeId).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                        var period = JsonConvert.DeserializeObject<ArrivedTimeViewModel>(result);
                        if (period != null)
                        {
                            ViewBag.Period = period.Period;
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
            HttpResponseMessage response = client.GetAsync(url1 + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<ReservationViewModel>(result);
                if (data != null)
                {
                    reservation = data;
                    HttpContext.Session.SetInt32("ReservationCreator", id);
                    List<ArrivedTimeViewModel> arrivedTimes = new List<ArrivedTimeViewModel>();
                    response = client.GetAsync(url2).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                        var periods = JsonConvert.DeserializeObject<List<ArrivedTimeViewModel>>(result);
                        if (periods != null)
                        {
                            arrivedTimes = periods;
                        }
                    }
                    arrivedTimes.Insert(0, new ArrivedTimeViewModel { Id = 0, Period = "請選擇時段" });
                    ViewBag.Periods = new SelectList(arrivedTimes, "Id", "Period");
                }
            }
            return View(reservation);
        }

        [HttpPost]
        public IActionResult Edit(ReservationViewModel reservation)
        {
            reservation.Id = HttpContext.Session.GetInt32("ReservationCreator").Value;
            string data = JsonConvert.SerializeObject(reservation);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(url1 + reservation.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Success");
            }
            List<ArrivedTimeViewModel> arrivedTimes = new List<ArrivedTimeViewModel>();
            response = client.GetAsync(url2).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var periods = JsonConvert.DeserializeObject<List<ArrivedTimeViewModel>>(result);
                if (periods != null)
                {
                    arrivedTimes = periods;
                }
            }
            arrivedTimes.Insert(0, new ArrivedTimeViewModel { Id = 0, Period = "請選擇時段" });
            ViewBag.Periods = new SelectList(arrivedTimes, "Id", "Period");
            return View(reservation);
        }

        public IActionResult Delete(int id)
        {
            HttpResponseMessage response = client.DeleteAsync(url1 + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return BadRequest();
        }
    }
}
