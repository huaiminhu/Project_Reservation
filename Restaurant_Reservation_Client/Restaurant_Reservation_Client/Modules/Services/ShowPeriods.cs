﻿using Restaurant_Reservation_Client.Models;
using Restaurant_Reservation_Client.Modules.IServices;

namespace Restaurant_Reservation_Client.Modules.Services
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
                // 若訂位日為今日, 需確定下拉式選單選項未超過訂位時段
                for (var i = 0; i < arrivalTimes.Count; i++)
                {
                    int remainSeat = 45 - reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                    var diff = Convert.ToInt32((DateTime.Parse(arrivalTimes[i].Period[0..5]) - DateTime.Now).TotalHours);
                    if (remainSeat > 0 && diff > 1)
                        results.Add(new DisplayViewModel { Id = arrivalTimes[i].Id, Display = arrivalTimes[i].Period + string.Format("\t(目前剩餘空位:{0})", remainSeat) });
                }
            }
            else
            {
                for (var i = 0; i < arrivalTimes.Count; i++)
                {
                    int remainSeat = 45 - reservations.Where(r => r.arrivalTimeId == arrivalTimes[i].Id).Sum(s => s.SeatRequirement);
                    if (remainSeat > 0)
                        results.Add(new DisplayViewModel { Id = arrivalTimes[i].Id, Display = arrivalTimes[i].Period + string.Format("\t(目前剩餘空位:{0})", remainSeat) });
                }
            }
            return results;
        }
    }
}
