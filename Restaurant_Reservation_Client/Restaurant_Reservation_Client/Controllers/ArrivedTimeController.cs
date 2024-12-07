using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant_Reservation_Client.Models;

namespace Restaurant_Reservation_Client.Controllers
{
    public class ArrivedTimeController : Controller
    {
        private string url = "https://localhost:7077/api/ArrivedTime/";
        private HttpClient client = new HttpClient();
        public List<ArrivedTimeViewModel> PeriodChoices()
        {
            List<ArrivedTimeViewModel> arrivedTimes = new List<ArrivedTimeViewModel>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if(response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<ArrivedTimeViewModel>> (result);
                if(data != null)
                {
                    arrivedTimes = data;
                }
            }
            return arrivedTimes;
        }
    }
}
