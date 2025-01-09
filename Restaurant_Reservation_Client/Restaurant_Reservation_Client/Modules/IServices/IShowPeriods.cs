using Restaurant_Reservation_Client.Models;

namespace Restaurant_Reservation_Client.Modules.IServices
{
    public interface IShowPeriods
    {
        List<DisplayViewModel> periods(DateTime selectedDate,
            List<DisplayViewModel> results,
            List<ReservationViewModel> reservations, 
            List<ArrivalTimeViewModel> arrivalTimes);
    }
}
