namespace Project14.ViewModels
{
    public class ShoppingCartItemViewModel
    {
        public int Id { get; set; }
        public string OrderGuid { get; set; }
        public int ProductId { get; set; } // 原為 string，已改為 int
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsApproved { get; set; }
    }
}
