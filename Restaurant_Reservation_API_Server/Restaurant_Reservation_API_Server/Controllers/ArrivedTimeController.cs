using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservation_Server.Repositories.Services;
using Restaurant_Reservation_API_Server.Repositories.Interfaces;

namespace Restaurant_Reservation_API_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArrivedTimeController : ControllerBase
    {
        private readonly IArrivedTimeRepository arrivedTimeRepository;
        public ArrivedTimeController(IArrivedTimeRepository arrivedTimeRepository)
        {
            this.arrivedTimeRepository = arrivedTimeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> AllReservations()
        {
            var data = await arrivedTimeRepository.AllArrivedTimes();
            return Ok(data);
        }
    }
}
