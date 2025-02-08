using Restaurant_Reservation_API_Server.Domain.Entities;

namespace Restaurant_Reservation_API_Server.Infrastructure.Repositories.Interfaces
{
    public interface IArrivalTimeRepository
    {
        // 讀取所有訂位時段
        Task<IEnumerable<ArrivalTime>> AllArrivalTimes();

        // 使用時段ID讀取訂位時段
        Task<ArrivalTime?> GetArrivalTime(int id);
    }
}
