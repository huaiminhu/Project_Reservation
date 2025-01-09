using Restaurant_Reservation_Client.Models;

namespace Restaurant_Reservation_Client.Modules.IServices
{
    public interface IReservationApiConsuming
    {
        List<ReservationViewModel> AllReservations(HttpClient client, string reservationApi);
        ReservationViewModel? FindReservation(int id, HttpClient client, string reservationApi);
        int Create(ReservationViewModel reservation, HttpClient client, string reservationApi);
        int Update(ReservationViewModel reservation, HttpClient client, string reservationApi);
        int Delete(int id, HttpClient client, string reservationApi);
        ReservationViewModel? ResByDateAndPhone(string newDate, string phone, HttpClient client, string reservationApi);
    }
}
