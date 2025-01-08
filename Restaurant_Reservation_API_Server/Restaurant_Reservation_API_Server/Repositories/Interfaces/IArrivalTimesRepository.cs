using Restaurant_Reservation_API_Server.Models;

namespace Restaurant_Reservation_API_Server.Repositories.Interfaces
{
    public interface IArrivalTimeRepository
    {
        Task<IEnumerable<ArrivalTime>> AllArrivalTimes();
        Task<ArrivalTime?> ArrivalTimeById(int id);
    }
}
