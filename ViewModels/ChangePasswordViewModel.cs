using System.ComponentModel.DataAnnotations;

namespace Project14.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name = "原始密碼")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "新密碼")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "新密碼與確認密碼不一致")]
        [Display(Name = "確認新密碼")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
