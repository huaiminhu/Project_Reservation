using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant_Reservation_Client.Models;

namespace Restaurant_Reservation_Client.Controllers
{
    public class ArrivedTimeController : Controller
    {
        private string url = "https://localhost:7077/api/ArrivedTime/";
        private HttpClient client = new HttpClient();
        public List<ArrivedTime> PeriodChoices()
        {
            List<ArrivedTime> arrivedTimes = new List<ArrivedTime>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if(response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<ArrivedTime>> (result);
                if(data != null)
                {
                    arrivedTimes = data;
                }
            }
            return arrivedTimes;
        }
    }
}
