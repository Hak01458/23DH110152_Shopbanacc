using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _23DH110152_Shopbanacc.Models.ViewModel
{
    public class RegisterVM     //lưu thông tin form đăng ký tài khoản khách hàng
    {
        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không trùng.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Họ và tên")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string CustomerPhone { get; set; }

        [Required]
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        public string CustomerEmail { get; set; }

        [Required]
        [Display(Name = "Địa chỉ")]
        public string  CustomerAddress { get; set; }
        public int CustomerID { get; internal set; }
    }
}