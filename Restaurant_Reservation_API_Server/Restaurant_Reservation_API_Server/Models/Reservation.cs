using System.ComponentModel.DataAnnotations;

namespace Reservation_Server.Models
{
    public class Reservation
    {

        [Key]
        public int Id { get; set; }
        
        public string BookingDate { get; set; }
        
        public string CustomerName { get; set; }
        
        public string Phone { get; set; }
        
        public string? ArrivedTime { get; set; }
        
        public int SeatRequirement { get; set; }
        
        public int ChildSeat { get; set; }
    }
}
