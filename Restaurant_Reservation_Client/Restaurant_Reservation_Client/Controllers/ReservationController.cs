using Microsoft.AspNetCore.Mvc;
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
        private string url2 = "https://localhost:7077/api/ArrivedTime/";
        private HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            List<ArrivedTime> arrivedTimes = new List<ArrivedTime>();
            HttpResponseMessage response = client.GetAsync(url2).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<ArrivedTime>>(result);
                if (data != null)
                {
                    arrivedTimes = data;
                    //int minus = 0;
                    //foreach (var p in reservations)
                    //{
                    //    minus += p.SeatRequirement;
                    //}
                    //ViewBag.Seats = 80 - minus;
                }
            }
            return View(arrivedTimes);
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
            HttpResponseMessage response = client.PostAsync(url1 + "FindResByPhone", content).Result;
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
            return View();
        }

        [HttpPost]
        public IActionResult MakeRes(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                string data = JsonConvert.SerializeObject(reservation);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(url1, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    var detail = JsonConvert.DeserializeObject<Reservation>(result);
                    int num = detail.Id;
                    HttpContext.Session.SetInt32("ReservationId", num);
                    return RedirectToAction("Success");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Success()
        {
            Reservation reservation = new();
            var id = HttpContext.Session.GetInt32("ReservationId");
            HttpResponseMessage response = client.GetAsync(url1 + id).Result;
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
