using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Reservation_Server.Models
{
    [Index(nameof(RealName), nameof(UserName), IsUnique = true)]
    public class Account
    {
        [Key]
        public int Id { get; set; }

        public string RealName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
