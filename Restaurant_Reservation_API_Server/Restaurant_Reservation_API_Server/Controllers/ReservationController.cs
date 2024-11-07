using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation_Server.Data;
using Reservation_Server.Models;
using Reservation_Server.Repositories.Interfaces;

namespace Reservation_Server.Controllers
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
        public async Task<ActionResult> AllReservations()
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
        public async Task<IActionResult> Update(int id)
        {
            var former = await reservationRepository.FindReservation(id);
            if(former == null)
                return NotFound();
            await reservationRepository.Update(former);
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
    }
}
