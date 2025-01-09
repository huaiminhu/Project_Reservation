using Restaurant_Reservation_Client.Models;
using Restaurant_Reservation_Client.Modules.IServices;

namespace Restaurant_Reservation_Client.Modules.Services
{
    public class ShowPeriods : IShowPeriods
    {
        public List<DisplayViewModel> ListOfPeriods(DateTime selectedDate,
            List<DisplayViewModel> results, 
            List<ReservationViewModel> reservations, 
            List<ArrivalTimeViewModel> arrivalTimes)
        {
            reservations = reservations.Where(r => r.BookingDate == selectedDate).ToList();
            if (selectedDate == DateTime.Today)
            {
                for (var i = 0; i < arrivalTimes.Count; i++)
                {
                    int remainSeat = 45 - reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                    var diff = Convert.ToInt32((DateTime.Parse(arrivalTimes[i].Period[0..5]) - DateTime.Now).TotalHours);
                    if (remainSeat > 0 && diff > 1)
                        results.Add(new DisplayViewModel { Id = arrivalTimes[i].Id, Display = arrivalTimes[i].Period + string.Format("\t(剩餘空位:{0})", remainSeat) });
                }
            }
            else
            {
                for (var i = 0; i < arrivalTimes.Count; i++)
                {
                    int remainSeat = 45 - reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                    if (remainSeat > 0)
                        results.Add(new DisplayViewModel { Id = arrivalTimes[i].Id, Display = arrivalTimes[i].Period + string.Format("\t(剩餘空位:{0})", remainSeat) });
                }
            }
            return results;
        }
    }
}
