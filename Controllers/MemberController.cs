using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web.Security;
using Project14.Helpers;
using Project14.Models;
using Project14.ViewModels;

namespace Project14.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private PChouseDBEntities context = new PChouseDBEntities();

        public ActionResult Index()
        {
            string userId = User.Identity.Name;
            var member = context.table_Member.FirstOrDefault(m => m.UserId == userId);
            ViewBag.MemberName = member?.Name;
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        [AllowAnonymous]
        public ActionResult Captcha()
        {
            string code = new Random().Next(1000, 9999).ToString();
            Session["Captcha"] = code;

            using (Bitmap bmp = new Bitmap(60, 30))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.DrawString(code, new Font("Arial", 16), Brushes.Black, 2, 2);
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    return File(ms.ToArray(), "image/png");
                }
            }
        }

        public ActionResult ShoppingCar(string sortBy = "default")
        {
            string userId = User.Identity.Name;
            var order = context.table_Order.FirstOrDefault(o => o.UserId == int.Parse(userId) && o.IsConfirmed == false);
            if (order == null)
                return View(new List<ShoppingCartItemViewModel>());

            var query = from od in context.table_OrderDetail
                        join p in context.table_Product on od.ProductId equals p.Id
                        where od.OrderGuid == order.OrderGuid
                        select new ShoppingCartItemViewModel
                        {
                            Id = od.Id,
                            OrderGuid = od.OrderGuid.ToString(),
                            ProductId = p.Id,
                            Name = p.Name,
                            Price = p.Price,
                            Quantity = od.Quantity,
                            IsApproved = od.IsApproved ?? false
                        };

            // 加上排序條件
            switch (sortBy)
            {
                case "name":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "price":
                    query = query.OrderByDescending(x => x.Price);
                    break;
                default:
                    query = query.OrderBy(x => x.Id); // 預設加入順序
                    break;
            }

            var cartItems = query.ToList();
            return View(cartItems);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShoppingCar(string Receiver, string Email, string Address)
        {
            string userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Home");

            var order = context.table_Order.FirstOrDefault(x => x.UserId == int.Parse(userId) && x.IsConfirmed == false);
            if (order == null)
            {
                TempData["Message"] = "找不到購物車訂單";
                return RedirectToAction("ShoppingCar");
            }

            var cartItems = (from od in context.table_OrderDetail
                             join p in context.table_Product on od.ProductId equals p.Id
                             where od.OrderGuid == order.OrderGuid
                             select new
                             {
                                 od.Quantity,
                                 p.Price
                             }).ToList();

            if (!cartItems.Any())
            {
                TempData["Message"] = "購物車是空的，無法下訂單";
                return RedirectToAction("ShoppingCar");
            }

            order.Receiver = Receiver;
            order.Email = Email;
            order.Address = Address;
            order.Date = DateTime.Now;
            order.IsConfirmed = true;

            foreach (var item in context.table_OrderDetail.Where(x => x.OrderGuid == order.OrderGuid))
                item.IsApproved = true;

            var totalAmount = cartItems.Sum(x => x.Price * x.Quantity);
            var payment = new table_PaymentLog
            {
                OrderGuid = order.OrderGuid,
                Amount = totalAmount,
                Status = "待付款",
                PaymentDate = DateTime.Now
            };
            context.table_PaymentLog.Add(payment);

            context.SaveChanges();
            TempData["Message"] = "✅ 訂單已送出！";
            return RedirectToAction("OrderList");
        }

        public ActionResult OrderList()
        {
            string userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Home");

            var ordersRaw = context.table_Order.Where(o => o.UserId == int.Parse(userId)).ToList();
            var orderDetails = context.table_OrderDetail.ToList();
            var paymentLogs = context.table_PaymentLog.ToList();
            var products = context.table_Product.ToList();

            var orders = ordersRaw.Select(o => new OrderSummaryViewModel
            {
                OrderGuid = o.OrderGuid.ToString(),
                TotalQuantity = orderDetails.Where(d => d.OrderGuid == o.OrderGuid).Sum(d => d.Quantity),
                TotalAmount = (from d in orderDetails
                               join p in products on d.ProductId equals p.Id
                               where d.OrderGuid == o.OrderGuid
                               select (p.Price) * d.Quantity).Sum(),
                Date = o.Date ?? DateTime.MinValue,
                PaymentStatus = paymentLogs.FirstOrDefault(p => p.OrderGuid == o.OrderGuid)?.Status ?? "未付款"
            })
            .OrderByDescending(o => o.Date)
            .ToList();

            return View(orders);
        }

        public ActionResult OrderDetail(string guid)
        {
            string userId = User.Identity.Name;
            var order = context.table_Order.FirstOrDefault(o => o.OrderGuid.ToString() == guid && o.UserId == int.Parse(userId));
            if (order == null) return RedirectToAction("OrderList");

            var items = (from d in context.table_OrderDetail
                         join p in context.table_Product on d.ProductId equals p.Id
                         where d.OrderGuid == order.OrderGuid
                         select new OrderDetailItemViewModel
                         {
                             ProductId = d.ProductId,
                             Name = p.Name,
                             Price = (int)p.Price,
                             Quantity = d.Quantity
                         }).ToList();

            var vm = new OrderDetailViewModel
            {
                OrderGuid = order.OrderGuid.ToString(),
                Date = order.Date,
                Receiver = order.Receiver,
                Email = order.Email,
                Address = order.Address,
                Items = items
            };
            return View(vm);
        }
        [HttpPost]
        public JsonResult AddCarAjax(int productId, int quantity)
        {
            string userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false, message = "未登入" });

            var order = context.table_Order.FirstOrDefault(x => x.UserId == int.Parse(userId) && x.IsConfirmed == false);
            if (order == null)
            {
                order = new table_Order
                {
                    OrderGuid = Guid.NewGuid(),
                    UserId = int.Parse(userId),
                    IsConfirmed = false,
                    Date = DateTime.Now
                };
                context.table_Order.Add(order);
                context.SaveChanges();
            }

            var existingItem = context.table_OrderDetail.FirstOrDefault(x =>
                x.OrderGuid == order.OrderGuid && x.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                context.table_OrderDetail.Add(new table_OrderDetail
                {
                    OrderGuid = order.OrderGuid,
                    ProductId = productId,
                    Quantity = quantity,
                    IsApproved = false
                });
            }

            context.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult UpdateCartQuantity(int id, int quantity)
        {
            if (quantity < 1)
                return Json(new { success = false, message = "數量不得小於 1" });

            var item = context.table_OrderDetail.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return Json(new { success = false, message = "找不到該筆資料" });

            item.Quantity = quantity;
            context.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        
        public JsonResult DeleteCarAjax(int id)
        {
            var item = context.table_OrderDetail.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return Json(new { success = false, message = "找不到該筆資料" });

            context.table_OrderDetail.Remove(item);
            context.SaveChanges();
            return Json(new { success = true });
        }
        
        [HttpGet]
        public ActionResult CancelOrder(string guid)
        {
            int uid = int.Parse(User.Identity.Name);
            string userId = User.Identity.Name;


            var order = context.table_Order
    .FirstOrDefault(o => o.OrderGuid.ToString() == guid
                      && o.UserId == uid
                      && o.IsConfirmed == true);

            if (order == null)
            {
                TempData["Message"] = "找不到此訂單或您無權操作。";
                return RedirectToAction("OrderList");
            }

            // 找付款紀錄（如已付款，不可取消）
            var payment = context.table_PaymentLog.FirstOrDefault(p => p.OrderGuid == order.OrderGuid);
            if (payment != null && payment.Status == "已付款")
            {
                TempData["Message"] = "此訂單已付款，無法取消。";
                return RedirectToAction("OrderList");
            }

            // 刪除付款紀錄（先刪除 FK 依賴關係）
            if (payment != null)
                context.table_PaymentLog.Remove(payment);

            // 刪除明細
            var details = context.table_OrderDetail.Where(d => d.OrderGuid == order.OrderGuid);
            context.table_OrderDetail.RemoveRange(details);

            // 刪除訂單
            context.table_Order.Remove(order);

            context.SaveChanges();

            TempData["Message"] = "❌ 訂單已成功取消。";
            return RedirectToAction("OrderList");
        }

        [HttpGet]
        public ActionResult Pay(string guid)
        {
            int uid = int.Parse(User.Identity.Name);
            string userId = User.Identity.Name;

            var order = context.table_Order
    .FirstOrDefault(o => o.OrderGuid.ToString() == guid
                      && o.UserId == uid
                      && o.IsConfirmed == true);

            if (order == null)
            {
                TempData["Message"] = "找不到訂單或尚未成立。";
                return RedirectToAction("OrderList");
            }

            var payment = context.table_PaymentLog
                .FirstOrDefault(p => p.OrderGuid == order.OrderGuid);

            if (payment == null)
            {
                TempData["Message"] = "找不到對應的付款紀錄。";
                return RedirectToAction("OrderList");
            }

            // 模擬付款完成（未串金流）
            payment.Status = "已付款";
           

            context.SaveChanges();

            TempData["Message"] = "✅ 付款成功，感謝您的訂購！";
            return RedirectToAction("OrderList");
        }
    }
}
