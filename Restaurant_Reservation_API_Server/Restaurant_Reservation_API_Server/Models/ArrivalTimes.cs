using System.ComponentModel.DataAnnotations;

namespace Restaurant_Reservation_API_Server.Models
{
    public class ArrivalTime // 訂位時段
    {
        [Key]
        public int Id { get; set; }
        public string Period { get; set; }   // 時段
    }
}
