using Restaurant_Reservation_Client.Models;

namespace Restaurant_Reservation_Client.Modules.IServices
{
    public interface IArrivalTimeApiConsuming
    {
        Task<List<ArrivalTimeViewModel>> AllArrivalTimes(HttpClient client, string arrivalTimeApi);
        Task<ArrivalTimeViewModel?> ArrivalTimeById(int id, HttpClient client, string arrivalTimeApi);
    }
}
