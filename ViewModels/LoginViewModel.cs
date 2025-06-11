using System.ComponentModel.DataAnnotations;

namespace Project14.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "使用者帳號")]
        public string UserId { get; set; }

        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "驗證碼")]
        public string Captcha { get; set; }

        [Display(Name = "記住我")]
        public bool RememberMe { get; set; }
    }
}
