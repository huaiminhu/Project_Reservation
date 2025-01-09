using Newtonsoft.Json;
using Restaurant_Reservation_Client.Models;
using Restaurant_Reservation_Client.Modules.IServices;

namespace Restaurant_Reservation_Client.Modules.Services
{
    public class ArrivalTimeApiConsuming : IArrivalTimeApiConsuming
    {
        public List<ArrivalTimeViewModel> AllArrivalTimes(HttpClient client, string arrivalTimeApi)
        {
            HttpResponseMessage responseForTimes = client.GetAsync(arrivalTimeApi).Result;
            if (responseForTimes.IsSuccessStatusCode)
            {
                string resultForTimes = responseForTimes.Content.ReadAsStringAsync().Result;
                var timesData = JsonConvert.DeserializeObject<List<ArrivalTimeViewModel>>(resultForTimes);
                if (timesData != null)
                {
                    return timesData;
                }
            }
            return null;
        }

        public ArrivalTimeViewModel? ArrivalTimeById(int id, HttpClient client, string arrivalTimeApi)
        {
            HttpResponseMessage responseForTime = client.GetAsync(arrivalTimeApi + id).Result;
            if (responseForTime.IsSuccessStatusCode)
            {
                string resultForTime = responseForTime.Content.ReadAsStringAsync().Result;
                var timeData = JsonConvert.DeserializeObject<ArrivalTimeViewModel>(resultForTime);
                if (timeData != null)
                    return timeData;
            }
            return null;
        }
    }
}
