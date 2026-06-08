using System.ComponentModel.DataAnnotations;

namespace GymManagement.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên hội viên không được để trống")]
        [Display(Name = "Họ và Tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại phải từ 10-11 số")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn gói tập")]
        [Display(Name = "Gói tập")]
        public string MembershipType { get; set; }

        [Range(1, 100000000, ErrorMessage = "Giá gói tập phải lớn hơn 0")]
        [Display(Name = "Giá tiền (VNĐ)")]
        public decimal Price { get; set; }
    }
}