namespace Project14.ViewModels
{
    public class ShoppingCartItem
    {
        public int ProductId { get; set; } // 原為 string，已改為 int
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
