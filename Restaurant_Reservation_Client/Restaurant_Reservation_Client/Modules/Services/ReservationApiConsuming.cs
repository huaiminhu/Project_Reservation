using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Restaurant_Reservation_Client.Models;
using Restaurant_Reservation_Client.Modules.IServices;
using System.Text;

namespace Restaurant_Reservation_Client.Modules.Services
{
    public class ReservationApiConsuming : IReservationApiConsuming
    {
        public List<ReservationViewModel> AllReservations(HttpClient client, string reservationApi)
        {
            HttpResponseMessage responseForReservations = client.GetAsync(reservationApi).Result;
            if (responseForReservations.IsSuccessStatusCode)
            {
                string resultForReservations = responseForReservations.Content.ReadAsStringAsync().Result;
                var reservationsData = JsonConvert.DeserializeObject<List<ReservationViewModel>>(resultForReservations);
                if (reservationsData != null)
                    return reservationsData;
            }
            return null;
        }

        public int Create(ReservationViewModel reservation, HttpClient client, string reservationApi)
        {
            string dataForCreate = JsonConvert.SerializeObject(reservation);
            StringContent content = new StringContent(dataForCreate, Encoding.UTF8, "application/json");
            HttpResponseMessage createResponse = client.PostAsync(reservationApi, content).Result;
            if (createResponse.IsSuccessStatusCode)
            {
                string createResult = createResponse.Content.ReadAsStringAsync().Result;
                var detail = JsonConvert.DeserializeObject<ReservationViewModel>(createResult);
                int id = detail.Id;
                return id;
            }
            return -1;
        }

        public int Delete(int id, HttpClient client, string reservationApi)
        {
            HttpResponseMessage response = client.DeleteAsync(reservationApi + id).Result;
            if (response.IsSuccessStatusCode)
                return 1;
            return -1;
        }

        public ReservationViewModel? FindReservation(int id, HttpClient client, string reservationApi)
        {
            HttpResponseMessage response = client.GetAsync(reservationApi + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<ReservationViewModel>(result);
                if (data != null)
                    return data;
            }
            return null;
        }

        public ReservationViewModel? ResByDateAndPhone(string newDate, string phone, HttpClient client, string reservationApi)
        {
            HttpResponseMessage responseForReservation = client.GetAsync(reservationApi + "ResByDateAndPhone?bookingDate=" + newDate + "&phone=" + phone).Result;
            if (responseForReservation.IsSuccessStatusCode)
            {
                string resultForReservation = responseForReservation.Content.ReadAsStringAsync().Result;
                var reservationData = JsonConvert.DeserializeObject<ReservationViewModel>(resultForReservation);
                if (reservationData != null)
                    return reservationData;
            }
            return null;
        }

        public int Update(ReservationViewModel reservation, HttpClient client, string reservationApi)
        {
            string dataForUpdate = JsonConvert.SerializeObject(reservation);
            StringContent content = new StringContent(dataForUpdate, Encoding.UTF8, "application/json");
            HttpResponseMessage responseForUpdate = client.PutAsync(reservationApi + reservation.Id, content).Result;
            if (responseForUpdate.IsSuccessStatusCode)
                return 1;
            return -1;
        }
    }
}
