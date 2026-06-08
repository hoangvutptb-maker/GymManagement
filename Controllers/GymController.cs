using Microsoft.AspNetCore.Mvc;
using GymManagement.Models;

namespace GymManagement.Controllers
{
    public class GymController : Controller
    {
        // Sử dụng static List để giữ dữ liệu khi chạy chương trình
        private static List<Member> members = new List<Member>()
        {
            new Member { Id = 1, Name = "Tô Hoàng Vũ", Email = "hoangvu@gmail.com", Phone = "0123456789", MembershipType = "Gói 1 Tháng", Price = 500000 },
            new Member { Id = 2, Name = "Tô Hoàng Long", Email = "hoanglong@gmail.com", Phone = "0123456788", MembershipType = "Gói 1 Năm", Price = 4500000 }
        };

        // 1. Hiển thị danh sách & Tìm kiếm (Mở rộng)
        public IActionResult Index(string searchString)
        {
            var result = members.AsEnumerable();
            if (!string.IsNullOrEmpty(searchString))
            {
                result = result.Where(s => s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }
            return View(result.ToList());
        }

        // 2. Xem chi tiết
        public IActionResult Details(int id)
        {
            var member = members.FirstOrDefault(m => m.Id == id);
            if (member == null) return NotFound();
            return View(member);
        }

        // 3. Thêm mới - GET
        public IActionResult Create()
        {
            return View();
        }

        // 3. Thêm mới - POST
        [HttpPost]
        public IActionResult Create(Member member)
        {
            if (ModelState.IsValid)
            {
                member.Id = members.Any() ? members.Max(m => m.Id) + 1 : 1;
                members.Add(member);
                TempData["Message"] = "Thêm mới hội viên thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // 4. Sửa - GET
        public IActionResult Edit(int id)
        {
            var member = members.FirstOrDefault(m => m.Id == id);
            if (member == null) return NotFound();
            return View(member);
        }

        // 4. Sửa - POST
        [HttpPost]
        public IActionResult Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                var index = members.FindIndex(m => m.Id == member.Id);
                if (index != -1)
                {
                    members[index] = member;
                    TempData["Message"] = "Cập nhật thông tin thành công!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(member);
        }

        // 5. Xóa (Dùng JavaScript để xác nhận trước khi gọi Action này)
        public IActionResult Delete(int id)
        {
            var member = members.FirstOrDefault(m => m.Id == id);
            if (member != null)
            {
                members.Remove(member);
                TempData["Message"] = "Đã xóa hội viên!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}