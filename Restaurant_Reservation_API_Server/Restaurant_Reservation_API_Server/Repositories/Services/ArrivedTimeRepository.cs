using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_API_Server.Data;
using Restaurant_Reservation_API_Server.Models;
using Restaurant_Reservation_API_Server.Repositories.Interfaces;

namespace Restaurant_Reservation_API_Server.Repositories.Services
{
    public class ArrivedTimeRepository : IArrivedTimeRepository
    {
        private readonly ReservationDbContext _ctx;

        public ArrivedTimeRepository(ReservationDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<IEnumerable<ArrivedTime>> AllArrivedTimes()
        {
            var data = await _ctx.arrivedTimes.ToListAsync();
            return data;
        }

        public async Task<ArrivedTime?> ArrivedTimeById(int id)
        {
            return await _ctx.arrivedTimes.FindAsync(id);
        }
    }
}
