using Restaurant_Reservation_API_Server.Models;

namespace Restaurant_Reservation_API_Server.Repositories.Interfaces
{
    public interface IarrivalTimeRepository
    {
        Task<IEnumerable<arrivalTime>> AllarrivalTimes();
        Task<arrivalTime?> arrivalTimeById(int id);
    }
}
