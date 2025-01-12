using Newtonsoft.Json;
using Restaurant_Reservation_Client.Models;
using Restaurant_Reservation_Client.Modules.IServices;

namespace Restaurant_Reservation_Client.Modules.Services
{
    public class ArrivalTimeApiConsuming : IArrivalTimeApiConsuming
    {
        // API串接用端點
        private string arrivalTimeApi = "https://localhost:7077/api/ArrivalTime/";

        private HttpClient client = new HttpClient();

        // 讀取所有訂位時段
        public async Task<List<ArrivalTimeViewModel>> AllArrivalTimes()
        {
            HttpResponseMessage responseForTimes = await client.GetAsync(arrivalTimeApi);
            if (responseForTimes.IsSuccessStatusCode)
            {
                string resultForTimes = await responseForTimes.Content.ReadAsStringAsync();
                var timesData = JsonConvert.DeserializeObject<List<ArrivalTimeViewModel>>(resultForTimes);
                if (timesData != null)
                {
                    return timesData;
                }
            }
            return null;
        }

        // 使用時段ID讀取訂位時段
        public async Task<ArrivalTimeViewModel?> ArrivalTimeById(int id)
        {
            HttpResponseMessage responseForTime = await client.GetAsync(arrivalTimeApi + id);
            if (responseForTime.IsSuccessStatusCode)
            {
                string resultForTime = await responseForTime.Content.ReadAsStringAsync();
                var timeData = JsonConvert.DeserializeObject<ArrivalTimeViewModel>(resultForTime);
                if (timeData != null)
                    return timeData;
            }
            return null;
        }
    }
}
