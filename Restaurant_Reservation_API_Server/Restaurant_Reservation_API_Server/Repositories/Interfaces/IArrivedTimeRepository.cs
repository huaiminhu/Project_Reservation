using Restaurant_Reservation_API_Server.Models;

namespace Restaurant_Reservation_API_Server.Repositories.Interfaces
{
    public interface IArrivedTimeRepository
    {
        Task<IEnumerable<ArrivedTime>> AllArrivedTimes();
    }
}
