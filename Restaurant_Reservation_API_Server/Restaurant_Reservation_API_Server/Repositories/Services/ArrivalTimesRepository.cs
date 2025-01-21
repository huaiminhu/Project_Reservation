using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_API_Server.Data;
using Restaurant_Reservation_API_Server.Models;
using Restaurant_Reservation_API_Server.Repositories.Interfaces;

namespace Restaurant_Reservation_API_Server.Repositories.Services
{
    public class ArrivalTimeRepository : IArrivalTimeRepository
    {
        private readonly ReservationDbContext _ctx;

        public ArrivalTimeRepository(ReservationDbContext ctx)
        {
            _ctx = ctx;
        }

        // 讀取所有訂位時段
        public async Task<IEnumerable<ArrivalTime>> AllArrivalTimes()
        {
            var data = await _ctx.ArrivalTimes.ToListAsync();
            return data;
        }

        // 使用時段ID讀取訂位時段
        public async Task<ArrivalTime?> GetArrivalTime(int id)
        {
            return await _ctx.ArrivalTimes.FindAsync(id);
        }
    }
}
