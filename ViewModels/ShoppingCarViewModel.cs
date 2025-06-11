using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project14.ViewModels
{
    public class ShoppingCarViewModel
    {
        [Required]
        [Display(Name = "收件人姓名")]
        public string Receiver { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "收件人信箱")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "收件人地址")]
        public string Address { get; set; }

        public List<ShoppingCartItemViewModel> CartItems { get; set; } = new List<ShoppingCartItemViewModel>();
    }
}
