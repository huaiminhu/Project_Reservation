using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_API_Server.Data;
using Restaurant_Reservation_API_Server.Models;
using Restaurant_Reservation_API_Server.Repositories.Interfaces;

namespace Restaurant_Reservation_API_Server.Repositories.Services
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
            var reservations = await _ctx.Reservations.ToListAsync();
            return reservations;
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
            var originalReservation = _ctx.Reservations.FirstOrDefault(x => x.Id == reservation.Id);
            if (originalReservation != null)
            {
                originalReservation.BookingDate = reservation.BookingDate;
                originalReservation.CustomerName = reservation.CustomerName;
                originalReservation.Phone = reservation.Phone;
                originalReservation.ArrivalTimeId = reservation.ArrivalTimeId;
                originalReservation.SeatRequirement = reservation.SeatRequirement;
                originalReservation.ChildSeat = reservation.ChildSeat;
            }
            await _ctx.SaveChangesAsync();
        }

        public Reservation? ResByDateAndPhone(DateTime bookingDate, string phone)
        {
            return _ctx.Reservations.FirstOrDefault(x => x.BookingDate == bookingDate && x.Phone == phone);
        }
    }
}
