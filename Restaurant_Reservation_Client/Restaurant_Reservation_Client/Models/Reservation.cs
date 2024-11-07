using System.ComponentModel.DataAnnotations;

namespace Reservation_Client.Models
{
    public class Reservation
    {
        
        [Required(ErrorMessage = "日期未輸入!")]
        [Display(Name = "請輸入訂位日期: ")]
        public string BookingDate { get; set; }


        [Required(ErrorMessage = "大名未輸入!")]
        [Display(Name = "請輸入訂位人姓名: ")]
        [MinLength(2)]
        public string CustomerName { get; set; }


        [Required(ErrorMessage = "連絡電話未輸入!")]
        [Display(Name = "請輸入訂位人聯絡電話: ")]
        [MinLength(8)]
        public string Phone { get; set; }


        [Required]
        public string? ArrivedTime { get; set; };


        [Required(ErrorMessage = "訂位人數未輸入!")]
        [Range(1, 40)]
        [Display(Name = "請輸入訂位人數: ")]
        public int SeatRequirement { get; set; }


        [Required(ErrorMessage = "兒童座椅數未輸入!")]
        [Range(0, 20)]
        [Display(Name = "請輸入兒童座椅需求數: ")]
        public int ChildSeat { get; set; }
    }
}
