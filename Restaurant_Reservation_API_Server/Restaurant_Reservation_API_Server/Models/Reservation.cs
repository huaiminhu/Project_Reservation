using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Restaurant_Reservation_API_Server.Models
{
    public class Reservation // 訂位資訊
    {

        [Key]
        public int Id { get; set; }        
        public DateTime BookingDate { get; set; }   // 訂位日期
        public string CustomerName { get; set; }   // 顧客姓名        
        public string Phone { get; set; }   // 連絡電話        
        public int SeatRequirement { get; set; }   // 座位需求        
        public int ChildSeat { get; set; }   // 兒童座椅需求
        
        // 訂位時段(外來鍵)
        public int ArrivalTimeId { get; set; }
        public ArrivalTime? ArrivalTime { get; set; }
    }
}
