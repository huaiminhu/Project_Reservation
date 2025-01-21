using Restaurant_Reservation_Client.Models;

namespace Restaurant_Reservation_Client.Modules.IServices
{
    public interface IReservationApiConsuming
    {
        // 讀取所有訂位資訊
        Task<List<ReservationViewModel>> AllReservations();

        //使用ID尋找訂位資訊
        Task<ReservationViewModel?> GetReservation(int id);

        // 新增訂位
        Task<int> Create(ReservationViewModel reservation);

        // 更新訂位資訊
        Task<int> Update(ReservationViewModel reservation);

        // 取消訂位
        Task<int> Delete(int id);

        // 使用日期及連絡電話查詢訂位資訊
        Task<ReservationViewModel?> ResByDateAndPhone(string newDate, string phone);
    }
}
