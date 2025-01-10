using Restaurant_Reservation_Client.Models;

namespace Restaurant_Reservation_Client.Modules.IServices
{
    public interface IReservationApiConsuming
    {
        // 讀取所有訂位資訊
        Task<List<ReservationViewModel>> AllReservations(HttpClient client, string reservationApi);

        //使用ID尋找訂位資訊
        Task<ReservationViewModel?> FindReservation(int id, HttpClient client, string reservationApi);

        // 新增訂位
        Task<int> Create(ReservationViewModel reservation, HttpClient client, string reservationApi);

        // 更新訂位資訊
        Task<int> Update(ReservationViewModel reservation, HttpClient client, string reservationApi);

        // 刪除訂位
        Task<int> Delete(int id, HttpClient client, string reservationApi);

        // 使用日期及連絡電話查詢訂位資訊
        Task<ReservationViewModel?> ResByDateAndPhone(string newDate, string phone, HttpClient client, string reservationApi);
    }
}
