using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Restaurant_Reservation_Client.Models;
using Restaurant_Reservation_Client.Modules.IServices;
using System.Text;

namespace Restaurant_Reservation_Client.Modules.Services
{
    public class ReservationApiConsuming : IReservationApiConsuming
    {
        public async Task<List<ReservationViewModel>> AllReservations(HttpClient client, string reservationApi)
        {
            HttpResponseMessage responseForReservations = await client.GetAsync(reservationApi);
            if (responseForReservations.IsSuccessStatusCode)
            {
                string resultForReservations = await responseForReservations.Content.ReadAsStringAsync();
                var reservationsData = JsonConvert.DeserializeObject<List<ReservationViewModel>>(resultForReservations);
                if (reservationsData != null)
                    return reservationsData;
            }
            return null;
        }

        public async Task<int> Create(ReservationViewModel reservation, HttpClient client, string reservationApi)
        {
            string dataForCreate = JsonConvert.SerializeObject(reservation);
            StringContent content = new StringContent(dataForCreate, Encoding.UTF8, "application/json");
            HttpResponseMessage createResponse = await client.PostAsync(reservationApi, content);
            if (createResponse.IsSuccessStatusCode)
            {
                string createResult = await createResponse.Content.ReadAsStringAsync();
                var detail = JsonConvert.DeserializeObject<ReservationViewModel>(createResult);
                int id = detail.Id;
                return id;
            }
            return -1;
        }

        public async Task<int> Delete(int id, HttpClient client, string reservationApi)
        {
            HttpResponseMessage response = await client.DeleteAsync(reservationApi + id);
            if (response.IsSuccessStatusCode)
                return 1;
            return -1;
        }

        public async Task<ReservationViewModel?> FindReservation(int id, HttpClient client, string reservationApi)
        {
            HttpResponseMessage response = await client.GetAsync(reservationApi + id);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ReservationViewModel>(result);
                if (data != null)
                    return data;
            }
            return null;
        }

        public async Task<ReservationViewModel?> ResByDateAndPhone(string newDate, string phone, HttpClient client, string reservationApi)
        {
            HttpResponseMessage responseForReservation = await client.GetAsync(reservationApi + "ResByDateAndPhone?bookingDate=" + newDate + "&phone=" + phone);
            if (responseForReservation.IsSuccessStatusCode)
            {
                string resultForReservation = await responseForReservation.Content.ReadAsStringAsync();
                var reservationData = JsonConvert.DeserializeObject<ReservationViewModel>(resultForReservation);
                if (reservationData != null)
                    return reservationData;
            }
            return null;
        }

        public async Task<int> Update(ReservationViewModel reservation, HttpClient client, string reservationApi)
        {
            string dataForUpdate = JsonConvert.SerializeObject(reservation);
            StringContent content = new StringContent(dataForUpdate, Encoding.UTF8, "application/json");
            HttpResponseMessage responseForUpdate = await client.PutAsync(reservationApi + reservation.Id, content);
            if (responseForUpdate.IsSuccessStatusCode)
                return 1;
            return -1;
        }
    }
}
