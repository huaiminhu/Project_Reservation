using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reservation_Client.Models;
using System.Security.Principal;
using System.Text;

namespace Reservation_Client.Controllers
{
    public class ReservationController : Controller
    {

        private string url = "https://localhost:7077/api/Reservation/";
        private HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            List<Reservation> reservations = new List<Reservation>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<Reservation>>(result);
                if (data != null)
                {
                    reservations = data;
                    int minus = 0;
                    foreach (var p in reservations)
                    {
                        
                    }
                    ViewBag.Seats = 40 - reservations.Count();
                    if (HttpContext.Session.GetString("UserSession") != null)
                    {
                        ViewBag.Seat = 60 - reservations.Count();
                        return View(reservations);
                    }
                }
            }
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
            HttpResponseMessage response = client.PostAsync(url + "FindResByPhone", content).Result;
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
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("ReservationId", reservation.Id.ToString());
                    return RedirectToAction("Success");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Success()
        {
            Reservation reservation = new Reservation();
            var id = HttpContext.Session.GetString("ReservationId");
            HttpResponseMessage response = client.GetAsync(url + id).Result;
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
