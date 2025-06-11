using System.Collections.Generic;

namespace Project14.ViewModels
{
    public class OrderListViewModel
    {
        public List<OrderDetailViewModel> Orders { get; set; } = new List<OrderDetailViewModel>();
    }
}