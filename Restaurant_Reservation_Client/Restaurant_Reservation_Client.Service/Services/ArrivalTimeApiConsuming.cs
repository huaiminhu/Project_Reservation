using Newtonsoft.Json;
using Restaurant_Reservation_Client.Model.ViewModels;
using Restaurant_Reservation_Client.Service.Services.IServices;

namespace Restaurant_Reservation_Client.Service.Services
{
    public class ArrivalTimeApiConsuming : IArrivalTimeApiConsuming
    {
        // API串接用端點
        private readonly string arrivalTimeApi = "https://localhost:7077/api/ArrivalTime/";

        private readonly HttpClient client = new();

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
            return [];
        }

        // 使用時段ID讀取訂位時段
        public async Task<ArrivalTimeViewModel?> GetArrivalTime(int id)
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
