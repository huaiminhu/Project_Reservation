using Microsoft.AspNetCore.Mvc;
using Restaurant_Reservation_API_Server.Domain.Entities;
using Restaurant_Reservation_API_Server.Infrastructure.Repositories.Interfaces;

namespace Restaurant_Reservation_API_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        // 依賴注入
        private readonly IReservationRepository reservationRepository;

        public ReservationController(IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
        }

        [HttpGet] // 讀取所有訂位資訊
        public async Task<IActionResult> AllReservations()
        {
            var reservations = await reservationRepository.AllReservations();
            return Ok(reservations); // 回傳200的HTTP RESPONSE和資料
        }

        [HttpGet("{id}")] //使用ID尋找訂位資訊
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await reservationRepository.GetReservation(id);
            if (reservation == null)
                return NotFound(); // 沒有找到則回傳404 HTTP RESPONSE
            return Ok(reservation);
        }

        [HttpPost] // 新增訂位
        public async Task<IActionResult> Create(Reservation reservation)
        {
            await reservationRepository.Create(reservation);

            // 新增成功將回傳201 HTTP RESPONSE
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")] // 更新訂位資訊
        public async Task<IActionResult> Update(int id, Reservation reservation)
        {
            // 使用CLIENT傳來的ID尋找原訂位資訊
            var originalReservation = await reservationRepository.GetReservation(id);
            if(originalReservation == null)
                return NotFound();

            // 更新原訂位資訊
            await reservationRepository.Update(reservation);
            return NoContent(); // 更新成功將回傳204 HTTP RESPONSE
        }

        [HttpDelete("{id}")] // 取消訂位
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await reservationRepository.GetReservation(id);
            if (reservation == null)
                return NotFound();
            await reservationRepository.Delete(reservation);
            return NoContent(); // 刪除成功將回傳204 HTTP RESPONSE
        }

        [HttpGet] // 使用日期及連絡電話查詢訂位資訊
        [Route("ResByDateAndPhone")]
        public ActionResult<Reservation> ResByDateAndPhone(DateTime bookingDate, string phone)
        {
            var reservation = reservationRepository.ResByDateAndPhone(bookingDate, phone);
            if(reservation == null) return NotFound();
            return Ok(reservation);
        }
    }
}
