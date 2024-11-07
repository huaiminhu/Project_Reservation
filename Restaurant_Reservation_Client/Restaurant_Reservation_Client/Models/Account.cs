using System.ComponentModel.DataAnnotations;

namespace Reservation_Client.Models
{
    public class Account
    {

        [Required(ErrorMessage = "員工姓名未輸入!")]
        [Display(Name = "員工姓名: ")]
        [MinLength(2)]
        public string RealName { get; set; }


        [Required(ErrorMessage = "使用者名稱未輸入!")]
        [Display(Name = "使用者名稱: ")]
        [MinLength(6)]
        public string UserName { get; set; }


        [Required(ErrorMessage = "密碼未輸入!")]
        [Display(Name = "密碼(至少1個數字、1個大寫或小寫英文字母以及一個特殊符號;長度在6-15之間): ")]
        [MinLength(6)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z])(?=.*\W).{6,15}$")]
        public string Password { get; set; }


        [Required(ErrorMessage = "密碼確認未輸入!")]
        [Compare("Password", ErrorMessage = "密碼確認未輸入!")]
        [Display(Name = "密碼再次確認: ")]
        [MinLength(6)]
        public string ConfirmPassword { get; set; }
    }
}
