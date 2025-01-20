using System.ComponentModel.DataAnnotations;

namespace Restaurant_Reservation_API_Server.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }
        public string CostumerName { get; set; }   // 會員名稱
        public string UserName { get; set; }   // 使用者名稱
        public string Password { get; set; }   // 密碼
        public string Phone { get; set; }   // 連絡電話
        public List<Activity>? Activities { get; set; }   // 優惠項目
        public List<Record>? Records { get; set; }   // 訂位紀錄
    }
}
