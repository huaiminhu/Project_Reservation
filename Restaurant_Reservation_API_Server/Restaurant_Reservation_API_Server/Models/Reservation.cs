using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Restaurant_Reservation_API_Server.Models
{
    [Index(nameof(Phone), IsUnique = true)]
    public class Reservation
    {

        [Key]
        public int Id { get; set; }        
        public DateTime BookingDate { get; set; }        
        public string CustomerName { get; set; }        
        public string Phone { get; set; }        
        public int SeatRequirement { get; set; }        
        public int ChildSeat { get; set; }
        
        public int arrivalTimeId { get; set; }
        public arrivalTime? arrivalTime { get; set; }
    }
}
