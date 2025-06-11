using System.ComponentModel.DataAnnotations;

namespace Project14.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "帳號")]
        public string UserId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "密碼長度至少 8 碼")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "兩次輸入的密碼不一致")]
        [Display(Name = "確認密碼")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "信箱")]
        public string Email { get; set; }
    }
}
