using Reservation_Server.Data;
using Reservation_Server.Models;
using Reservation_Server.Repositories.Interfaces;

namespace Reservation_Server.Repositories.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ReservationDbContext _ctx;

        public AccountRepository(ReservationDbContext ctx)
        {
            _ctx = ctx;    
        }

        public async Task AddNewEmployee(Account account)
        {
            await _ctx.Accounts.AddAsync(account);
            await _ctx.SaveChangesAsync();
        }

        public int FindEmployee(Account account)
        {
            int exist = 0;
            var data = _ctx.Accounts.FirstOrDefault(x => x.UserName == account.UserName && x.Password == account.Password);
            if (data != null)
                exist += 1;
            return exist;
        }
    }
}
