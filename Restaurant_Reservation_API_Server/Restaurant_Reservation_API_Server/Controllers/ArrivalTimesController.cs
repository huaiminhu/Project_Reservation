﻿using Microsoft.AspNetCore.Mvc;
using Restaurant_Reservation_API_Server.Infrastructure.Repositories.Interfaces;
using Restaurant_Reservation_API_Server.Domain.Entities;

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

        [HttpGet] // 讀取所有訂位時段
        public async Task<IActionResult> AllArrivalTimes()
        {
            var data = await ArrivalTimeRepository.AllArrivalTimes();
            return Ok(data);
        }

        [HttpGet("{id}")] // 使用時段ID讀取訂位時段
        public async Task<ActionResult<ArrivalTime>> GetArrivalTime(int id)
        {
            var data = await ArrivalTimeRepository.GetArrivalTime(id);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

    }
}
