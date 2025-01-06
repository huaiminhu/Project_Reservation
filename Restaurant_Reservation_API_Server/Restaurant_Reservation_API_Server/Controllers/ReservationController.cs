using Microsoft.AspNetCore.Mvc;
using Restaurant_Reservation_API_Server.Models;
using Restaurant_Reservation_API_Server.Repositories.Interfaces;

namespace Restaurant_Reservation_API_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository reservationRepository;

        public ReservationController(IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> AllReservations()
        {
            var data = await reservationRepository.AllReservations();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> FindReservation(int id)
        {
            var data = await reservationRepository.FindReservation(id);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            await reservationRepository.Create(reservation);
            return CreatedAtAction(nameof(FindReservation), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Reservation reservation)
        {
            var former = await reservationRepository.FindReservation(reservation.Id);
            if(former == null)
                return NotFound();
            await reservationRepository.Update(reservation);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await reservationRepository.FindReservation(id);
            if (data == null)
                return NotFound();
            await reservationRepository.Delete(data);
            return NoContent();
        }

        [HttpGet]
        [Route("FindByDateAndPhone")]
        public ActionResult<Reservation> FindByDateAndPhone(DateTime bookingDate, string phone)
        {
            var data = reservationRepository.FindByDateAndPhone(bookingDate, phone);
            if(data == null) return NotFound();
            return Ok(data);
        }
    }
}
