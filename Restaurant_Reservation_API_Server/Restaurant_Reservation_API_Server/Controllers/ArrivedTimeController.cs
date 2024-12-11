using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Reservation_API_Server.Repositories.Services;
using Restaurant_Reservation_API_Server.Repositories.Interfaces;
using Restaurant_Reservation_API_Server.Models;

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
        public async Task<IActionResult> AllArrivedTimes()
        {
            var data = await arrivedTimeRepository.AllArrivedTimes();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArrivedTime>> ArrivedTimeById(int id)
        {
            var data = await arrivedTimeRepository.ArrivedTimeById(id);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

    }
}
