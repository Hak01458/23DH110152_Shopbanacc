using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Remoting.Messaging;
using _23DH110152_Shopbanacc.Models;
using _23DH110152_Shopbanacc.Models.ViewModel;
using System.Web.Management;
using System.Web.Security;

namespace _23DH110152_Shopbanacc.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Register
        private MyStoreEntities db = new MyStoreEntities();
        public ActionResult Register()
        {
            return View();
        }
        // POST: account/register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // kiểm tra xem tên đăng nhập đã tồn tại chưa
                var existingUser = db.Users.FirstOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại!!");
                    return View(model);
                }
                // nếu chưa tồn tại thì tạo bàn ghi thông tin tài khoản trong bảng user
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password, //lưu ý nên mã hóa mật khẩu trước khi lưu
                    UserRole = "Customer"
                };
                db.Users.Add(user);
                //và tạo bảng ghi thông tin khách hàng trong bảng Customer
                var customer = new Customer
                {
                    CustomerID = model.CustomerID,
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username,
                };
                db.Customers.Add(customer);
                //Lưu thông tin tài khoản và  thông tin khách hàng vào CSDL
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        public ActionResult Login()
        {
            return View();
        }
        //post: account/login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if(!ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u=> u.Username == model.Username
                && u.Password == model.Password
                && u.UserRole == "Customer");
                if (user != null)
                {
                    //lưu trạng thái đăng nhập vào session
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.UserRole;
                    //lưu thông tin xác thực người dùng vào cookie
                    FormsAuthentication.SetAuthCookie(user.Username,false);
                    return RedirectToAction("Index" , "Home"); 

                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}