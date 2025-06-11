using System;

namespace Project14.ViewModels
{
    public class OrderSummaryViewModel
    {
        public string OrderGuid { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public string PaymentStatus { get; set; }
    }
}
