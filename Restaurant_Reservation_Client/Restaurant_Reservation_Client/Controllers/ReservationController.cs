using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Reservation_Client.Models;
using Restaurant_Reservation_Client.Controllers;
using Restaurant_Reservation_Client.Models;
using System.Security.Principal;
using System.Text;

namespace Reservation_Client.Controllers
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
            List<string> seats = new List<string>();
            List<Reservation> reservations = new List<Reservation>();
            HttpResponseMessage response1 = client1.GetAsync(url1).Result;
            List<ArrivedTime> arrivedTimes = new List<ArrivedTime>();
            HttpResponseMessage response2 = client2.GetAsync(url2).Result;
            if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
            {
                string result1 = response1.Content.ReadAsStringAsync().Result;
                string result2 = response2.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<Reservation>>(result1);
                var time = JsonConvert.DeserializeObject<List<ArrivedTime>>(result2);
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
                                if (arrivedTimes[i].Period == reservations[j].ArrivedTime)
                                {
                                    requirement += reservations[j].SeatRequirement;
                                }
                            }
                            seats.Add(arrivedTimes[i].Period + string.Format("{:} 人", 40 - requirement));
                        }
                        seats.Add(arrivedTimes[i].Period + " : 40 人");
                    }
                }
            }
            ViewBag.Seats = seats;
            return View();
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string phone)
        {
            Reservation reservation = new Reservation();
            StringContent content = new StringContent(phone, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client1.PostAsync(url1 + "FindResByPhone", content).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Reservation>(result);
                if (data != null)
                    reservation = data;
            }
            return View(reservation);
        }

        [HttpGet]
        public IActionResult MakeRes()
        {
            List<ArrivedTime> arrivedTimes = new List<ArrivedTime>();
            HttpResponseMessage response = client2.GetAsync(url2).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<ArrivedTime>>(result);
                if(data != null)
                {
                    arrivedTimes = data;
                }
            }
            ViewBag.Periods = new SelectList(arrivedTimes, "TimeId", "Period");
            return View();
        }

        [HttpPost]
        public IActionResult MakeRes(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                string data = JsonConvert.SerializeObject(reservation);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client1.PostAsync(url1, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    var detail = JsonConvert.DeserializeObject<Reservation>(result);
                    int num = detail.Id;
                    HttpContext.Session.SetInt32("ReservationId", num);
                    return RedirectToAction("Success");
                }
            }
            return View(reservation);
        }

        [HttpGet]
        public IActionResult Success()
        {
            Reservation reservation = new();
            var id = HttpContext.Session.GetInt32("ReservationId");
            HttpResponseMessage response = client1.GetAsync(url1 + id).Result;
            if(response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Reservation>(result);
                if (data != null)
                    reservation = data;
            }
            return View(reservation);
        }

    }
}
