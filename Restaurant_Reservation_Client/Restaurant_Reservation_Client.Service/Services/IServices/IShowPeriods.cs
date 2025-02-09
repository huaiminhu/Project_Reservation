using Restaurant_Reservation_Client.Model.ViewModels;

namespace Restaurant_Reservation_Client.Service.Services.IServices
{
    public interface IShowPeriods
    {
        // 顯示可選訂位時段
        List<DisplayViewModel> ListOfPeriods(DateTime selectedDate,
            List<DisplayViewModel> results,
            List<ReservationViewModel> reservations, 
            List<ArrivalTimeViewModel> arrivalTimes);
    }
}
