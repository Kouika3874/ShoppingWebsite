using System;
using System.Collections.Generic;
using System.Linq;

namespace Project14.ViewModels
{
    public class OrderDetailViewModel
    {
        public string OrderGuid { get; set; }
        public DateTime? Date { get; set; }
        public string Receiver { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<OrderDetailItemViewModel> Items { get; set; }

        public int Total => Items?.Sum(x => x.Price * x.Quantity) ?? 0;
        public bool IsPaid { get; set; }
    }

    public class OrderDetailItemViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; } // 新增：圖片檔名屬性
    }
}
