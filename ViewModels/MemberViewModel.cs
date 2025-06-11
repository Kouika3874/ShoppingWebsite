using System.ComponentModel.DataAnnotations;

namespace Project14.ViewModels
{
    public class MemberViewModel
    {
       

        [Display(Name = "會員帳號")]
        public string UserId { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "電子郵件")]
        public string Email { get; set; }
    }
}
