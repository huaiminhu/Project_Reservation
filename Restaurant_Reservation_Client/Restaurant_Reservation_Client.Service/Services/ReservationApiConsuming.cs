﻿using Newtonsoft.Json;
using Restaurant_Reservation_Client.Model.ViewModels;
using Restaurant_Reservation_Client.Service.Services.IServices;
using System.Text;

namespace Restaurant_Reservation_Client.Service.Services
{
    public class ReservationApiConsuming : IReservationApiConsuming
    {
        // API串接用端點
        private string reservationApi = "https://localhost:7077/api/Reservation/";

        private HttpClient client = new HttpClient();

        // 讀取所有訂位資訊
        public async Task<List<ReservationViewModel>> AllReservations()
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

        // 新增訂位
        public async Task<int> Create(ReservationViewModel reservation)
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

        // 取消訂位
        public async Task<int> Delete(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync(reservationApi + id);
            if (response.IsSuccessStatusCode)
                return 1;
            return -1;
        }

        //使用ID尋找訂位資訊
        public async Task<ReservationViewModel?> GetReservation(int id)
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

        // 使用日期及連絡電話查詢訂位資訊
        public async Task<ReservationViewModel?> ResByDateAndPhone(string newDate, string phone)
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

        // 更新訂位資訊
        public async Task<int> Update(ReservationViewModel reservation)
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
