using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reservation_Client.Models;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Reservation_Client.Controllers
{
    public class AccountController : Controller
    {

        private string url = "https://localhost:7079/api/Account/";
        private HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(Account account)
        {
            if(ModelState.IsValid)
            {
                string data = JsonConvert.SerializeObject(account);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(url + "Employee", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("UserSession", account.UserName);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public IActionResult LogOut()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Index");
            }   
            return View();
        }
    }
}
