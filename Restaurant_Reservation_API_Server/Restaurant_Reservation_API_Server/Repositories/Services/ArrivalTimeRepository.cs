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
        public async Task<IEnumerable<ArrivalTime>> AllArrivalTimes()
        {
            var data = await _ctx.ArrivalTimes.ToListAsync();
            return data;
        }

        public async Task<ArrivalTime?> ArrivalTimeById(int id)
        {
            return await _ctx.ArrivalTimes.FindAsync(id);
        }
    }
}
