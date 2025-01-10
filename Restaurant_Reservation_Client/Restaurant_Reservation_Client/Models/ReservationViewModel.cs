using System.ComponentModel.DataAnnotations;

namespace Restaurant_Reservation_Client.Models
{
    public class ReservationViewModel   // 訂位資訊
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "日期未輸入!")]
        [Display(Name = "訂位日期"), DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }


        [Required(ErrorMessage = "大名未輸入!")]
        [Display(Name = "訂位人姓名")]
        [MinLength(2, ErrorMessage = "名字至少2個字吧!")]
        public string CustomerName { get; set; }


        [Required(ErrorMessage = "連絡電話未輸入!")]
        [Display(Name = "訂位人聯絡電話")]
        [MinLength(8, ErrorMessage = "這電話長度不對!")]
        public string Phone { get; set; }


        [Required(ErrorMessage = "訂位人數未輸入!")]
        [Range(1, 35, ErrorMessage = "訂位人數須介於1到35之間!")]
        [Display(Name = "訂位人數")]
        public int SeatRequirement { get; set; }


        [Required(ErrorMessage = "兒童座椅數未輸入!")]
        [Range(0, 15, ErrorMessage = "兒童座椅數須介於0到15之間!")]
        [Display(Name = "兒童座椅需求數")]
        public int ChildSeat { get; set; }


        [Display(Name = "用餐時段")]
        public int arrivalTimeId { get; set; }

    }
}
