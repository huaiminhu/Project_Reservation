using Microsoft.AspNetCore.Mvc;
using Restaurant_Reservation_API_Server.Repositories.Interfaces;
using Restaurant_Reservation_API_Server.Models;

namespace Restaurant_Reservation_API_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class arrivalTimeController : ControllerBase
    {
        private readonly IarrivalTimeRepository arrivalTimeRepository;
        public arrivalTimeController(IarrivalTimeRepository arrivalTimeRepository)
        {
            this.arrivalTimeRepository = arrivalTimeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> AllarrivalTimes()
        {
            var data = await arrivalTimeRepository.AllarrivalTimes();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<arrivalTime>> arrivalTimeById(int id)
        {
            var data = await arrivalTimeRepository.arrivalTimeById(id);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

    }
}
