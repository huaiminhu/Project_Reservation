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
        private HttpClient client1 = new HttpClient();
        private string url2 = "https://localhost:7077/api/ArrivedTime/";
        private HttpClient client2 = new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            List<SeatsViewModel> seats = new List<SeatsViewModel>();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            HttpResponseMessage response1 = client1.GetAsync(url1).Result;
            List<ArrivedTimeViewModel> arrivedTimes = new List<ArrivedTimeViewModel>();
            HttpResponseMessage response2 = client2.GetAsync(url2).Result;
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
                            for (var j = 0; j <= reservations.Count; j++)
                            {
                                if (arrivedTimes[i].Id == reservations[j].ArrivedTimeId)
                                {
                                    requirement += reservations[j].SeatRequirement;
                                }
                            }
                            seats.Add(new SeatsViewModel{ Period = arrivedTimes[i].Period, RemainSeats = 40 - requirement });
                        }
                        seats.Add(new SeatsViewModel { Period = arrivedTimes[i].Period, RemainSeats = 40 });
                    }
                }
            }
            //ViewBag.Seats = seats;
            return View(seats);
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
            HttpResponseMessage response = client1.PostAsync(url1 + "FindResByPhone", content).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<ReservationViewModel>(result);
                if (data != null)
                    reservation = data;
            }
            return View(reservation);
        }

        [HttpGet]
        public IActionResult MakeRes()
        {
            List<ArrivedTimeViewModel> arrivedTimes = new List<ArrivedTimeViewModel>();
            HttpResponseMessage response = client2.GetAsync(url2).Result;
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
            List<ArrivedTimeViewModel> arrivedTimes = new List<ArrivedTimeViewModel>();
            HttpResponseMessage response1 = client2.GetAsync(url2).Result;
            if (response1.IsSuccessStatusCode)
            {
                string result1 = response1.Content.ReadAsStringAsync().Result;
                var data1 = JsonConvert.DeserializeObject<List<ArrivedTimeViewModel>>(result1);
                if (data1 != null)
                {
                    arrivedTimes = data1;
                }
            }
            arrivedTimes.Insert(0, new ArrivedTimeViewModel { Id = 0, Period = "請選擇時段" });
            ViewBag.Periods = new SelectList(arrivedTimes, "Id", "Period");
            if (ModelState.IsValid)
            {
                string data2 = JsonConvert.SerializeObject(reservation);
                StringContent content = new StringContent(data2, Encoding.UTF8, "application/json");
                HttpResponseMessage response2 = client1.PostAsync(url1, content).Result;
                if (response2.IsSuccessStatusCode)
                {
                    string result2 = response2.Content.ReadAsStringAsync().Result;
                    var detail = JsonConvert.DeserializeObject<ReservationViewModel>(result2);
                    string phone = detail.Phone;
                    HttpContext.Session.SetString("ReservationCreator", phone);
                    return RedirectToAction("Success");
                }
            }
            return View(reservation);
        }

        [HttpGet]
        public IActionResult Success()
        {
            ReservationViewModel reservation = new();
            var id = HttpContext.Session.GetString("ReservationCreator");
            HttpResponseMessage response = client1.GetAsync(url1 + id).Result;
            if(response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<ReservationViewModel>(result);
                if (data != null)
                    reservation = data;
            }
            return View(reservation);
        }

    }
}
