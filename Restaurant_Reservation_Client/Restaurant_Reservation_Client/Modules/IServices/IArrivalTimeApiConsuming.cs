using Restaurant_Reservation_Client.Models;

namespace Restaurant_Reservation_Client.Modules.IServices
{
    public interface IArrivalTimeApiConsuming
    {
        List<ArrivalTimeViewModel> AllArrivalTimes(HttpClient client, string arrivalTimeApi);
        ArrivalTimeViewModel? ArrivalTimeById(int id, HttpClient client, string arrivalTimeApi);
    }
}
