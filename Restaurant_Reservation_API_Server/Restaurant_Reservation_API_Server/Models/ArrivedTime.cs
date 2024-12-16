using System.ComponentModel.DataAnnotations;

namespace Restaurant_Reservation_API_Server.Models
{
    public class ArrivedTime
    {
        [Key]
        public int Id { get; set; }
        public string Period { get; set; }

        //public ICollection<Reservation>? Reservations { get; set; }
    }
}
