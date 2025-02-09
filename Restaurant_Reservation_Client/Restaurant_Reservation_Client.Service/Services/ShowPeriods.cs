using Restaurant_Reservation_Client.Model.ViewModels;
using Restaurant_Reservation_Client.Service.Services.IServices;

namespace Restaurant_Reservation_Client.Service.Services
{
    public class ShowPeriods : IShowPeriods
    {
        // 顯示可選訂位時段
        public List<DisplayViewModel> ListOfPeriods(DateTime selectedDate,
            List<DisplayViewModel> results, 
            List<ReservationViewModel> reservations, 
            List<ArrivalTimeViewModel> arrivalTimes)
        {
            // 篩選出所有訂位日期為指定日期的所有訂位資訊
            reservations = reservations.Where(r => r.BookingDate == selectedDate).ToList();
            
            // 判定訂位日期是否為今日
            if (selectedDate == DateTime.Today)
            {
                // 若訂位日為今日, 需確定下拉式選單選項訂位時段是否已過
                for (var i = 0; i < arrivalTimes.Count; i++)
                {
                    int remainSeat = 45 - reservations.Where(r => r.ArrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                    var diff = Convert.ToInt32((DateTime.Parse(arrivalTimes[i].Period[0..5]) - DateTime.Now).TotalHours);
                    if (remainSeat > 0 && diff > 0)
                        results.Add(new DisplayViewModel { Id = arrivalTimes[i].Id, Display = arrivalTimes[i].Period + string.Format("\t(目前剩餘空位:{0})", remainSeat) });
                }
            }
            else
            {
                for (var i = 0; i < arrivalTimes.Count; i++)
                {
                    int remainSeat = 45 - reservations.Where(r => r.ArrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                    if (remainSeat > 0)
                        results.Add(new DisplayViewModel { Id = arrivalTimes[i].Id, Display = arrivalTimes[i].Period + string.Format("\t(目前剩餘空位:{0})", remainSeat) });
                }
            }
            return results;
        }
    }
}
