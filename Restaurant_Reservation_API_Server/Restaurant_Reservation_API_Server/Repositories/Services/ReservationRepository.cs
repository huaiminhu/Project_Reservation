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

        // 讀取所有訂位資訊
        public async Task<IEnumerable<Reservation>> AllReservations()
        {
            var reservations = await _ctx.Reservations.ToListAsync();
            return reservations;
        }

        // 新增訂位
        public async Task Create(Reservation reservation)
        {
            await _ctx.Reservations.AddAsync(reservation);
            await _ctx.SaveChangesAsync();
        }

        // 刪除訂位
        public async Task Delete(Reservation reservation)
        {
            _ctx.Reservations.Remove(reservation);
            await _ctx.SaveChangesAsync();
        }

        // 使用ID尋找訂位資訊
        public async Task<Reservation?> FindReservation(int id)
        {
            return await _ctx.Reservations.FindAsync(id);
        }

        // 更新訂位資訊
        public async Task Update(Reservation reservation)
        {
            // 使用模型ID尋找原訂位資訊
            var originalReservation = _ctx.Reservations.FirstOrDefault(x => x.Id == reservation.Id);
            
            // 若有找到原訂位資訊將更新並儲存
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

        // 使用日期及連絡電話查詢訂位資訊
        public Reservation? ResByDateAndPhone(DateTime bookingDate, string phone)
        {
            return _ctx.Reservations.FirstOrDefault(x => x.BookingDate == bookingDate && x.Phone == phone);
        }
    }
}
