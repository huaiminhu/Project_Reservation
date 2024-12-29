using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Restaurant_Reservation_Client.Models;
using System.Linq;
using System.Text;

namespace Restaurant_Reservation_Client.Controllers
{
    public class ReservationController : Controller
    {

        private string url1 = "https://localhost:7077/api/Reservation/";
        private string url2 = "https://localhost:7077/api/arrivalTime/";
        private HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            List<SeatsViewModel> seats = new List<SeatsViewModel>();
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            HttpResponseMessage response1 = client.GetAsync(url1).Result;
            List<arrivalTimeViewModel> arrivalTimes = new List<arrivalTimeViewModel>();
            HttpResponseMessage response2 = client.GetAsync(url2).Result;
            if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
            {
                string result1 = response1.Content.ReadAsStringAsync().Result;
                string result2 = response2.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<ReservationViewModel>>(result1);
                var time = JsonConvert.DeserializeObject<List<arrivalTimeViewModel>>(result2);
                if (data != null && time != null)
                {
                    var modifiedData = data.Where(r => r.BookingDate == DateTime.Today.ToString("d").Replace('/', '-')).ToList();
                    reservations = modifiedData;
                    arrivalTimes = time;
                    for (var i = 0; i < arrivalTimes.Count; i++)
                    {
                        int requirement = 0;
                        if(reservations.Count != 0) 
                        {
                            for (var j = 0; j <= reservations.Count - 1; j++)
                            {
                                if (arrivalTimes[i].Id == reservations[j].arrivalTimeId)
                                {
                                    requirement += reservations[j].SeatRequirement;
                                }
                            }
                            seats.Add(new SeatsViewModel { Period = arrivalTimes[i].Period, RemainSeats = 40 - requirement });
                        }
                        else
                            seats.Add(new SeatsViewModel { Period = arrivalTimes[i].Period, RemainSeats = 40 });
                    }
                }
            }
            return View(seats);
        }

        [HttpGet]
        public IActionResult MakeRes(string selectedDate)
        {
            List<int> invalidSeats = new List<int>();
            List <ReservationViewModel> reservations = new List<ReservationViewModel>();
            HttpResponseMessage getListResponse = client.GetAsync(url1).Result;
            List<arrivalTimeViewModel> arrivalTimes = new List<arrivalTimeViewModel>();
            HttpResponseMessage response = client.GetAsync(url2).Result;
            if (response.IsSuccessStatusCode && getListResponse.IsSuccessStatusCode)
            {
                string getListResult = getListResponse.Content.ReadAsStringAsync().Result;
                var listOfData = JsonConvert.DeserializeObject<List<ReservationViewModel>>(getListResult);
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<arrivalTimeViewModel>>(result);
                if(data != null && listOfData != null)
                {
                    var modifiedData = listOfData.Where(r => r.BookingDate == selectedDate).ToList();
                    reservations = modifiedData;
                    for (int i = 0; i <= data.Count; i++)
                    {
                        //從這裡開始CODING, 首頁MODAL選定日期後..., 若想換日期, 在日期欄位下方新增同一個MODAL
                        if (Convert.ToInt32((DateTime.Parse(data[i].Period[0..5]) - DateTime.Now).TotalHours) > 1 && )
                    }
                }
            }
            arrivalTimes.Insert(0, new arrivalTimeViewModel { Id = 0, Period = "請選擇時段" });
            ViewBag.Periods = new SelectList(arrivalTimes, "Id", "Period");
            return View();
        }

        [HttpPost]
        public IActionResult MakeRes(ReservationViewModel reservation)
        {
            List<ReservationViewModel> reservations = new List<ReservationViewModel>();
            HttpResponseMessage getListResponse = client.GetAsync(url1).Result;
            HttpResponseMessage createResponse = new();
            if (getListResponse.IsSuccessStatusCode)
            {
                string getListResult = getListResponse.Content.ReadAsStringAsync().Result;
                var listOfData = JsonConvert.DeserializeObject<List<ReservationViewModel>>(getListResult);
                if (listOfData != null)
                {
                    var modifiedData = listOfData.Where(r => r.BookingDate == reservation.BookingDate && r.arrivalTimeId == reservation.arrivalTimeId).ToList();
                    reservations = modifiedData;
                    var invalidSeats = reservations.Sum(s => s.SeatRequirement);
                    if (invalidSeats + reservation.SeatRequirement > 40)
                    {
                        ViewBag.NoMoreSeat = "您選擇的時段座位數不夠了><~請換個時段吧";
                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            string data = JsonConvert.SerializeObject(reservation);
                            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                            createResponse = client.PostAsync(url1, content).Result;
                            if (createResponse.IsSuccessStatusCode)
                            {
                                string result = createResponse.Content.ReadAsStringAsync().Result;
                                var detail = JsonConvert.DeserializeObject<ReservationViewModel>(result);
                                int id = detail.Id;
                                HttpContext.Session.SetInt32("ReservationCreator", id);
                                return RedirectToAction("Success");
                            }
                        }
                    }
                }
            }
            List<arrivalTimeViewModel> arrivalTimes = new List<arrivalTimeViewModel>();
            HttpResponseMessage response = client.GetAsync(url2).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<arrivalTimeViewModel>>(result);
                if (data != null)
                {
                    arrivalTimes = data;
                }
            }
            arrivalTimes.Insert(0, new arrivalTimeViewModel { Id = 0, Period = "請選擇時段" });
            ViewBag.Periods = new SelectList(arrivalTimes, "Id", "Period");
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
                    response = client.GetAsync(url2 + reservation.arrivalTimeId).Result;
                    if( response.IsSuccessStatusCode )
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                        var period = JsonConvert.DeserializeObject<arrivalTimeViewModel>(result);
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
                    response = client.GetAsync(url2 + reservation.arrivalTimeId).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                        var period = JsonConvert.DeserializeObject<arrivalTimeViewModel>(result);
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
                    List<arrivalTimeViewModel> arrivalTimes = new List<arrivalTimeViewModel>();
                    response = client.GetAsync(url2).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                        var periods = JsonConvert.DeserializeObject<List<arrivalTimeViewModel>>(result);
                        if (periods != null)
                        {
                            arrivalTimes = periods;
                        }
                    }
                    arrivalTimes.Insert(0, new arrivalTimeViewModel { Id = 0, Period = "請選擇時段" });
                    ViewBag.Periods = new SelectList(arrivalTimes, "Id", "Period");
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
            List<arrivalTimeViewModel> arrivalTimes = new List<arrivalTimeViewModel>();
            response = client.GetAsync(url2).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var periods = JsonConvert.DeserializeObject<List<arrivalTimeViewModel>>(result);
                if (periods != null)
                {
                    arrivalTimes = periods;
                }
            }
            arrivalTimes.Insert(0, new arrivalTimeViewModel { Id = 0, Period = "請選擇時段" });
            ViewBag.Periods = new SelectList(arrivalTimes, "Id", "Period");
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
