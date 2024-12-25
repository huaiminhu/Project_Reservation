using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_API_Server.Data;
using Restaurant_Reservation_API_Server.Models;
using Restaurant_Reservation_API_Server.Repositories.Interfaces;

namespace Restaurant_Reservation_API_Server.Repositories.Services
{
    public class arrivalTimeRepository : IarrivalTimeRepository
    {
        private readonly ReservationDbContext _ctx;

        public arrivalTimeRepository(ReservationDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<IEnumerable<arrivalTime>> AllarrivalTimes()
        {
            var data = await _ctx.arrivalTimes.ToListAsync();
            return data;
        }

        public async Task<arrivalTime?> arrivalTimeById(int id)
        {
            return await _ctx.arrivalTimes.FindAsync(id);
        }
    }
}
