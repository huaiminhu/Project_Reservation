using Reservation_Server.Models;

namespace Reservation_Server.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task AddNewEmployee(Account account);
        int FindEmployee(Account account);
    }
}
