using Restaurant_Reservation_Client.Models;

namespace Restaurant_Reservation_Client.Modules.IServices
{
    public interface IReservationApiConsuming
    {
        Task<List<ReservationViewModel>> AllReservations(HttpClient client, string reservationApi);
        Task<ReservationViewModel?> FindReservation(int id, HttpClient client, string reservationApi);
        Task<int> Create(ReservationViewModel reservation, HttpClient client, string reservationApi);
        Task<int> Update(ReservationViewModel reservation, HttpClient client, string reservationApi);
        Task<int> Delete(int id, HttpClient client, string reservationApi);
        Task<ReservationViewModel?> ResByDateAndPhone(string newDate, string phone, HttpClient client, string reservationApi);
    }
}
