using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Project14.Helpers;
using Project14.Models;
using Project14.ViewModels;

namespace Project14.Controllers
{
    public class HomeController : Controller
    {
        PChouseDBEntities context = new PChouseDBEntities();

        // 首頁 - 顯示所有商品
        public ActionResult Index()
        {
            var products = context.table_Product
                .Select(p => new ProductListItemViewModel
                {
                    ProductId = p.Id,       // ✅ 修正欄位名稱
                    Name = p.Name,
                    Image = p.Image,
                    Price = p.Price         // ✅ 若 Price 為 decimal，不需 ??
                }).ToList();

            return View(products);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel vm)
        {
            System.Diagnostics.Debug.WriteLine("【使用者輸入】");
            System.Diagnostics.Debug.WriteLine("帳號：" + vm.UserId);
            System.Diagnostics.Debug.WriteLine("密碼（明文）：" + vm.Password);
            System.Diagnostics.Debug.WriteLine("密碼（雜湊）：" + PasswordHelper.HashPassword(vm.Password));

            if (!ModelState.IsValid) return View(vm);

            // 驗證碼驗證
            string sessionCaptcha = Session["Captcha"] as string;
            if (string.IsNullOrEmpty(sessionCaptcha) || sessionCaptcha != vm.Captcha)
            {
                ViewBag.Message = "驗證碼錯誤，請重新輸入。";
                return View(vm);
            }

            // 雜湊密碼
            string inputPassword = vm.Password.Trim();
            string hashedPassword = PasswordHelper.HashPassword(inputPassword);

            // 比對帳號與密碼（不分大小寫）
            int uid = int.Parse(vm.UserId.Trim());
            string userId = vm.UserId.Trim().ToLower();
            var member = context.table_Member
    .FirstOrDefault(m => m.UserId == uid && m.Password == hashedPassword);

            if (member != null)
            {
                Session["Welcome"] = $"歡迎您 {member.Name}";
                FormsAuthentication.SetAuthCookie(member.UserName, vm.RememberMe);
                Session["UserId"] = member.UserId;
                Session["Captcha"] = null; // 清除驗證碼
                return RedirectToAction("Index", "Member");
            }

            ViewBag.Message = "帳號或密碼錯誤，請重新輸入。";
            return View(vm);
        }

        // 註冊頁（GET）
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // 註冊處理（POST）
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // 檢查帳號是否存在
            if (context.table_Member.Any(m => m.UserId == vm.UserId))
            {
                ModelState.AddModelError("UserId", "帳號已存在");
                return View(vm);
            }

            // 建立實體對象
            var member = new table_Member
            {
                UserId = vm.UserId,
                Password = PasswordHelper.HashPassword(vm.Password),
                Name = vm.Name,
                Email = vm.Email
            };

            context.table_Member.Add(member);
            context.SaveChanges();
            return RedirectToAction("Login");
        }
    }
}
