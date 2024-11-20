using Microsoft.EntityFrameworkCore;
using Reservation_Server.Models;

namespace Reservation_Server.Data
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options)
        {

        }

        public DbSet<Reservation> Reservations { get; set; }
    }
}
