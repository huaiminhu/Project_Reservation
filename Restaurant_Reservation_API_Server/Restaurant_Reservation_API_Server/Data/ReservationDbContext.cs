using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_API_Server.Models;

namespace Restaurant_Reservation_API_Server.Data
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options)
        {

        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ArrivalTime> ArrivalTimes { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Activity> activities { get; set; }

        // 每次啟動網站都預先產生所有訂位時段
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArrivalTime>().HasData(
                new { Id = 1, Period = "11:30 - 13:00" },
                new { Id = 2, Period = "13:00 - 14:30" },
                new { Id = 3, Period = "14:30 - 16:00" },
                new { Id = 4, Period = "16:00 - 17:30" },
                new { Id = 5, Period = "17:30 - 19:00" },
                new { Id = 6, Period = "19:00 - 20:30" }
                );
        }
    }
}
