using System.Linq;
using System.Web.Mvc;
using Project14.Models;
using Project14.ViewModels;

namespace Project14.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        PChouseDBEntities context = new PChouseDBEntities();

        public ActionResult Index()
        {
            var products = context.table_Product
                .Select(p => new ProductListItemViewModel
                {
                    ProductId = p.Id,       // ✅ 修正為正確的欄位名稱
                    Name = p.Name,
                    Image = p.Image,
                    Price = p.Price         // ✅ 已為 decimal，無需 ??
                }).ToList();

            return View(products);
        }
    }
}
