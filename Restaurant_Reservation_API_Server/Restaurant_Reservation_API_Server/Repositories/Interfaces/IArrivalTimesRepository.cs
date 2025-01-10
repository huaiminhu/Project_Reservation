using Restaurant_Reservation_API_Server.Models;

namespace Restaurant_Reservation_API_Server.Repositories.Interfaces
{
    public interface IArrivalTimeRepository
    {
        // 讀取所有訂位時段
        Task<IEnumerable<ArrivalTime>> AllArrivalTimes();

        // 使用時段ID讀取訂位時段
        Task<ArrivalTime?> ArrivalTimeById(int id);
    }
}
