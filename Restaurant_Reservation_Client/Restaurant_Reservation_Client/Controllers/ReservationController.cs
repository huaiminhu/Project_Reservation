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
                                if (arrivedTimes[i].Id == reservations[j].ArrivedTimeId)
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
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string phone)
        {
            ReservationViewModel reservation = new ReservationViewModel();
            StringContent content = new StringContent(phone, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url1 + "FindResByPhone", content).Result;
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
            ArrivedTimeViewModel arrivedTime = new();
            string periodId = reservation.ArrivedTimeId.ToString();            
            HttpResponseMessage response1 = client.GetAsync(url2 + periodId).Result;
            if (response1.IsSuccessStatusCode)
            {
                string result1 = response1.Content.ReadAsStringAsync().Result;
                var data1 = JsonConvert.DeserializeObject<ArrivedTimeViewModel>(result1);
                if (data1 != null)
                    arrivedTime = data1;
            }
            reservation.ArrivedTime = arrivedTime;
            if (ModelState.IsValid)
            {
                string data2 = JsonConvert.SerializeObject(reservation);
                StringContent content2 = new StringContent(data2, Encoding.UTF8, "application/json");
                HttpResponseMessage response2 = client.PostAsync(url1, content2).Result;
                if (response2.IsSuccessStatusCode)
                {
                    string result2 = response2.Content.ReadAsStringAsync().Result;
                    var detail = JsonConvert.DeserializeObject<ReservationViewModel>(result2);
                    int id = detail.Id;
                    HttpContext.Session.SetInt32("ReservationCreator", id);
                    return RedirectToAction("Success");
                }
            }
            List<ArrivedTimeViewModel> arrivedTimes = new List<ArrivedTimeViewModel>();
            HttpResponseMessage response3 = client.GetAsync(url2).Result;
            if (response3.IsSuccessStatusCode)
            {
                string result3 = response3.Content.ReadAsStringAsync().Result;
                var data3 = JsonConvert.DeserializeObject<List<ArrivedTimeViewModel>>(result3);
                if (data3 != null)
                {
                    arrivedTimes = data3;
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
                    reservation = data;
            }
            return View(reservation);
        }

    }
}
