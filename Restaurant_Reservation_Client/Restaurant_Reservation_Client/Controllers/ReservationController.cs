using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reservation_Client.Models;
using System.Text;

namespace Reservation_Client.Controllers
{
    public class ReservationController : Controller
    {

        private string url = "https://localhost:7077/api/Reservation/";
        private HttpClient client = new HttpClient();
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Booking()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Booking(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                string data = JsonConvert.SerializeObject(reservation);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Success");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Success(int id)
        {
            Reservation reservation = new Reservation();
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
