using Microsoft.AspNetCore.Mvc;
using Restaurant_Reservation_API_Server.Models;
using Restaurant_Reservation_API_Server.Repositories.Interfaces;

namespace Restaurant_Reservation_API_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        // 使用分層服務
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
        public async Task<ActionResult<Reservation>> FindReservation(int id)
        {
            var reservation = await reservationRepository.FindReservation(id);
            if (reservation == null)
                return NotFound(); // 沒有找到則回傳404 HTTP RESPONSE
            return Ok(reservation);
        }

        [HttpPost] // 新增訂位
        public async Task<IActionResult> Create(Reservation reservation)
        {
            await reservationRepository.Create(reservation);

            // 新增成功將回傳201 HTTP RESPONSE
            return CreatedAtAction(nameof(FindReservation), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")] // 更新訂位資訊
        public async Task<IActionResult> Update(Reservation reservation)
        {
            // 使用CLIENT傳來的ID尋找原訂位資訊
            var originalReservation = await reservationRepository.FindReservation(reservation.Id);
            if(originalReservation == null)
                return NotFound();

            // 更新原訂位資訊
            await reservationRepository.Update(reservation);
            return NoContent(); // 更新成功將回傳204 HTTP RESPONSE
        }

        [HttpDelete("{id}")] // 取消訂位
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await reservationRepository.FindReservation(id);
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
