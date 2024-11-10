using _23DH110152_Shopbanacc.Models;
using _23DH110152_Shopbanacc.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _23DH110152_Shopbanacc.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        // GET: Order/Checkout
        [Authorize]
        public ActionResult Checkout()
        {
            //Kiểm tra giỏ hàng trong session,
            //nếu giỏ hàng rổng hoặc không có sản phần thì chuyển hướng về trang chủ
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }
            //Xác thực người dùng đã đăng nhập chưa, nếu chưa thi chuyển hướng tới trăng Đăng nhập
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null) { return RedirectToAction("Login", "Account"); }

            //Lẩy thông tin khách hàng từ CSDL, nếu chưa có thi chuyển hướng tới trang Đăng nhập 
            //nếu có rồi thi lấy địa chỉ của khách hàng và gán vào ShippingAddress của CheckoutVM
            var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
            if (customer == null) { return RedirectToAction("Login", "Account"); }
            var model = new CheckoutVM
            { //Tạo dữ liệu hiển thị cho CheckoutVM
                CartItems = cart, //Lấy danh sách các sản phẩn trong giỏ hàng
                TotalAmount = cart.Sum(item => item.TotalPrice),//tổng giá trị của các mặt hàng trong giỏ hàng
                OrderDate = DateTime.Now, //Mặc định lấy bằng thời điểm đặt hàng
                ShippingAddress = customer.CustomerAddress, // Lấy địa chỉ mặc định từ bảng Customer
                CustomerID = customer.CustomerID, // Lấy nã khách hàng từ bảng Customer
                Username = customer.Username// Lấy tên đăng nhập từ bảng Customer
            };
            return View(model);
        }
        // POST: Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Checkout(CheckoutVM model)
        {

            if (ModelState.IsValid)
            {
                var cart = Session["Cart"] as List<CartItem>;
                //Nếu giò hàng rỗng sẽ điều hướng tới trang Home
                if (cart == null || !cart.Any()) { return RedirectToAction("Index", "Home"); }
                //Nếu người dùng chưa đăng nhập sẽ điều hướng tới trang Login
                var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
                if (user == null) { return RedirectToAction("Login", "Account"); }
                // nếu khách hàng không khớp với tên tên đăng nhập, sẽ điều hướng tới trang Login
                var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
                if (customer == null) { return RedirectToAction("Login", "Account"); }
                //Nếu người dùng chọn thanh toán bằng Paypal, sẽ điều hướng tới trang PaymentWithPaypal
                if (model.PaymentMethod == "Paypal")
                {
                    return RedirectToAction("PaymentWithPaypal", "PayPal", model);
                }
                // Thiết lập PaymentStatus dựa trên PaymentMethod
                string paymentStatus = "Chưa thanh toán";
                switch (model.PaymentMethod)
                {
                    case "Tiền mặt": paymentStatus = "Thanh toán tiền mặt"; break;
                    case "Paypal": paymentStatus = "Thanh toán paypal"; break;
                    case "Mua trước trả sau": paymentStatus = "Trà góp"; break;
                    default: paymentStatus = "Chứa thanh toán"; break;
                }
                //Tạo đơn hàng và chi tiết đơn hàng liên quan
                var order = new Order
                {
                    CustomerID = customer.CustomerID,
                    OrderDate = model.OrderDate,
                    TotalAmount = model.TotalAmount,
                    PaymentStatus = paymentStatus,
                    PaymentMethod = model.PaymentMethod,
                    ShippingMethod = model.ShippingMethod,
                    ShippingAddress = model.ShippingAddress,
                    OrderDetails = cart.Select(item => new OrderDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToList()
                };
                // Lưu đơn hàng vào CSDL
                db.Orders.Add(order);
                db.SaveChanges();
                // Xóa giỏ hàng sau khi đặt hàng thành công
                Session["Cart"] = null;
                // Điều hướng tới trang Xác nhận đơn hàng
                return RedirectToAction("OrderSuccess", new { id = order.OrderID });
            }
            return View(model);
        }
    }
}