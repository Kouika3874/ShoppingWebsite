namespace Project14.ViewModels
{
    public class AddToCartViewModel
    {
        public int ProductId { get; set; }      // 原為 string，已修正為 int
        public int Quantity { get; set; } = 1;
    }
}
