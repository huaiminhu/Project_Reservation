using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservation_Server.Models;
using Reservation_Server.Repositories.Interfaces;

namespace Reservation_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEmployee(Account account)
        {
            await accountRepository.AddNewEmployee(account);
            return CreatedAtAction(nameof(FindEmployee), new { id = account.Id }, account);
        }

        [HttpPost]
        [Route("Employee")]
        public IActionResult FindEmployee(Account account)
        {
            var data = accountRepository.FindEmployee(account);
            if (data == 0)
                return NotFound();
            return Ok();
        }
    }
}
