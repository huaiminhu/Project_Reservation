using Restaurant_Reservation_API_Server.Models;

namespace Restaurant_Reservation_API_Server.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        // 讀取所有訂位資訊
        Task<IEnumerable<Reservation>> AllReservations();

        // 使用ID尋找訂位資訊
        Task<Reservation?> GetReservation(int id);

        // 新增訂位
        Task Create(Reservation reservation);

        // 更新訂位資訊
        Task Update(Reservation reservation);

        // 取消訂位
        Task Delete(Reservation reservation);

        // 使用日期及連絡電話查詢訂位資訊
        Reservation? ResByDateAndPhone(DateTime bookingDate, string phone);
    }
}
