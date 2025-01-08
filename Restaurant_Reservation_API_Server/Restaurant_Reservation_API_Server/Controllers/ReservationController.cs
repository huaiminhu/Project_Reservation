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
            var reservations = await reservationRepository.AllReservations();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> FindReservation(int id)
        {
            var reservation = await reservationRepository.FindReservation(id);
            if (reservation == null)
                return NotFound();
            return Ok(reservation);
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
            var originalReservation = await reservationRepository.FindReservation(reservation.Id);
            if(originalReservation == null)
                return NotFound();
            await reservationRepository.Update(reservation);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await reservationRepository.FindReservation(id);
            if (reservation == null)
                return NotFound();
            await reservationRepository.Delete(reservation);
            return NoContent();
        }

        [HttpGet]
        [Route("ResByDateAndPhone")]
        public ActionResult<Reservation> ResByDateAndPhone(DateTime bookingDate, string phone)
        {
            var reservation = reservationRepository.ResByDateAndPhone(bookingDate, phone);
            if(reservation == null) return NotFound();
            return Ok(reservation);
        }
    }
}
