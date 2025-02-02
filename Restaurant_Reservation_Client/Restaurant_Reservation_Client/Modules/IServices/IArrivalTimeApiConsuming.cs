﻿using Restaurant_Reservation_Client.Models;

namespace Restaurant_Reservation_Client.Modules.IServices
{
    public interface IArrivalTimeApiConsuming
    {
        // 讀取所有訂位時段
        Task<List<ArrivalTimeViewModel>> AllArrivalTimes();
        
        // 使用時段ID讀取訂位時段
        Task<ArrivalTimeViewModel?> GetArrivalTime(int id);
    }
}
