namespace Project14.ViewModels
{
    public class ProductListItemViewModel
    {
        public int ProductId { get; set; }           // 原為 string，已修正為 int
        public string Name { get; set; }             // 商品名稱
        public string Image { get; set; }            // 圖片路徑
        public decimal Price { get; set; }           // 商品價格
        public int? CategoryId { get; set; }         // 類別代碼
        public string CategoryName { get; set; }     // 類別名稱
    }
}
