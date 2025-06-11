using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Project14.Models;
using Project14.ViewModels;
using Project14.Helpers;

namespace Project14.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private PChouseDBEntities context = new PChouseDBEntities();

        // ===== 後台首頁 =====
        public ActionResult Index()
        {
            return View();
        }

        // ===== 登入與登出 =====
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View(new AdminLoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminLoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            string hash = PasswordHelper.HashPassword(vm.Password);
            var admin = context.table_Admin.FirstOrDefault(a => a.AdminId == vm.UserName && a.Password == hash);

            if (admin != null)
            {
                FormsAuthentication.SetAuthCookie(admin.AdminId, vm.RememberMe);
                return RedirectToAction("Index");
            }

            ViewBag.Message = "帳號或密碼錯誤";
            return View(vm);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        // ===== 商品管理 =====
        public ActionResult ProductList()
        {
            var products = context.table_Product.ToList();
            return View(products);
        }

        [HttpGet]
        public ActionResult CreateProduct()
        {
            ViewBag.CategoryList = new SelectList(context.table_Category, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(table_Product product)
        {
            if (ModelState.IsValid)
            {
                context.table_Product.Add(product);
                context.SaveChanges();
                return RedirectToAction("ProductList");
            }
            ViewBag.CategoryList = new SelectList(context.table_Category, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            var product = context.table_Product.Find(id);
            if (product == null) return HttpNotFound();
            ViewBag.CategoryList = new SelectList(context.table_Category, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(table_Product model)
        {
            if (ModelState.IsValid)
            {
                context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("ProductList");
            }
            ViewBag.CategoryList = new SelectList(context.table_Category, "CategoryId", "Name", model.CategoryId);
            return View(model);
        }

        public ActionResult DeleteProduct(int id)
        {
            var product = context.table_Product.Find(id);
            if (product != null)
            {
                context.table_Product.Remove(product);
                context.SaveChanges();
            }
            return RedirectToAction("ProductList");
        }

        public ActionResult Inventory()
        {
            var products = context.table_Product.ToList();
            return View(products);
        }

        // ===== 會員管理 =====
        public ActionResult MemberList()
        {
            var members = context.table_Member
                .Select(m => new MemberViewModel
                {
                    UserId = m.UserId,
                    Name = m.Name,
                    Email = m.Email
                }).ToList();

            return View(members);
        }

        public ActionResult MemberOrders(string userId)
        {
            var orders = context.table_Order
    .Where(o => o.UserId == int.Parse(userId))
    .OrderByDescending(o => o.Date)
    .ToList();
            ViewBag.UserId = userId;
            return View("OrderList", orders);
        }

        public ActionResult DeleteMember(string userId)
        {
            var member = context.table_Member.FirstOrDefault(m => m.UserId == int.Parse(userId));
            if (member != null)
            {
                var orders = context.table_Order.Where(o => o.UserId == int.Parse(userId)).ToList();
                foreach (var order in orders)
                {
                    var details = context.table_OrderDetail.Where(d => d.OrderGuid == order.OrderGuid).ToList();
                    context.table_OrderDetail.RemoveRange(details);

                    var payments = context.table_PaymentLog.Where(p => p.OrderGuid == order.OrderGuid).ToList();
                    context.table_PaymentLog.RemoveRange(payments);
                }
                context.table_Order.RemoveRange(orders);
                context.table_Member.Remove(member);
                context.SaveChanges();
            }
            return RedirectToAction("MemberList");
        }

        // ===== 訂單明細 =====
        public ActionResult OrderDetail(string guid)
        {
            var items = (from d in context.table_OrderDetail
                         join p in context.table_Product on d.ProductId equals p.Id
                         where d.OrderGuid.ToString() == guid
                         select new OrderDetailItemViewModel
                         {
                             ProductId = p.Id,
                             Name = p.Name,
                             Price = (int)p.Price,
                             Quantity = d.Quantity
                         }).ToList();

            var order = context.table_Order.FirstOrDefault(o => o.OrderGuid.ToString() == guid);

            var vm = new OrderDetailViewModel
            {
                OrderGuid = guid,
                Receiver = order.Receiver,
                Email = order.Email,
                Address = order.Address,
                Date = order.Date,
                Items = items
            };

            return View(vm);
        }

        // ===== 訂單管理清單（共用） =====
        public ActionResult OrderList()
        {
            var orders = context.table_Order.OrderByDescending(o => o.Date).ToList();
            return View(orders);
        }
    }
}
