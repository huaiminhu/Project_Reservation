using System.ComponentModel.DataAnnotations;

namespace Restaurant_Reservation_API_Server.Models
{
    public class Record
    {
        [Key]
        public int Id { get; set; }
        
        // 訂位日期
        public DateTime BookingDate { get; set; }   

        // 訂位時段
        public int ArrivalTimeId { get; set; }   
        public ArrivalTime? ArrivalTime { get; set; }

        // 會員
        public int MemberId { get; set; }   
        public Member? Member { get; set; }
    }
}
