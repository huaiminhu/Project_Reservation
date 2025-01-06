using Microsoft.AspNetCore.Mvc;
using Restaurant_Reservation_API_Server.Repositories.Interfaces;
using Restaurant_Reservation_API_Server.Models;

namespace Restaurant_Reservation_API_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArrivalTimeController : ControllerBase
    {
        private readonly IArrivalTimeRepository ArrivalTimeRepository;
        public ArrivalTimeController(IArrivalTimeRepository ArrivalTimeRepository)
        {
            this.ArrivalTimeRepository = ArrivalTimeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> AllArrivalTimes()
        {
            var data = await ArrivalTimeRepository.AllArrivalTimes();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArrivalTime>> ArrivalTimeById(int id)
        {
            var data = await ArrivalTimeRepository.ArrivalTimeById(id);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

    }
}
