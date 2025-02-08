using System.ComponentModel.DataAnnotations;

namespace Restaurant_Reservation_API_Server.Domain.Entities
{
    public class ArrivalTime // 訂位時段
    {
        [Key]
        public int Id { get; set; }
        public required string Period { get; set; }   // 時段
    }
}
