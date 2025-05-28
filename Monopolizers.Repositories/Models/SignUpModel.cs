using System.ComponentModel.DataAnnotations;

namespace Monopolizers.Repository.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Họ không được để trống.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Tên không được để trống.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống.")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; }

    }
}
