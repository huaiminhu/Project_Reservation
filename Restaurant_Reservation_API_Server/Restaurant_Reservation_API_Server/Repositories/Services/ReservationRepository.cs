using Microsoft.EntityFrameworkCore;
using Reservation_Server.Data;
using Reservation_Server.Models;
using Reservation_Server.Repositories.Interfaces;

namespace Reservation_Server.Repositories.Services
{
    public class ReservationRepository : IReservationRepository
    {

        private readonly ReservationDbContext _ctx;

        public ReservationRepository(ReservationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<Reservation>> AllReservations()
        {
            var data = await _ctx.Reservations.ToListAsync();
            return data;
        }

        public async Task Create(Reservation reservation)
        {
            await _ctx.Reservations.AddAsync(reservation);
            await _ctx.SaveChangesAsync();
        }

        public async Task Delete(Reservation reservation)
        {
            _ctx.Reservations.Remove(reservation);
            await _ctx.SaveChangesAsync();
        }

        public async Task<Reservation?> FindReservation(int id)
        {
            return await _ctx.Reservations.FindAsync(id);
        }

        public async Task Update(Reservation reservation)
        {
            _ctx.Reservations.Update(reservation);
            await _ctx.SaveChangesAsync();
        }

        public Reservation? FindByPhone(string phone)
        {
            return _ctx.Reservations.FirstOrDefault(x => x.Phone == phone);
        }
    }
}
