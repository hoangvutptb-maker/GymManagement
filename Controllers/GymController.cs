using Microsoft.AspNetCore.Mvc;
using GymManagement.Models;
using GymManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Controllers
{
    public class GymController : Controller
    {
        // Inject DbContext thông qua constructor
        private readonly GymManagementContext _context;

        public GymController(GymManagementContext context)
        {
            _context = context;
        }

        // 1. Hiển thị danh sách & Tìm kiếm
        // Truy vấn toàn bộ members từ database
        // Nếu có searchString, lọc theo tên (không phân biệt chữ hoa/thường)
        public async Task<IActionResult> Index(string searchString)
        {
            // Bắt đầu truy vấn
            IQueryable<Member> membersQuery = _context.Members;

            // Nếu searchString không rỗng, thêm điều kiện lọc
            if (!string.IsNullOrEmpty(searchString))
            {
                membersQuery = membersQuery.Where(s => 
                    s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            // Thực thi truy vấn bất đồng bộ và trả về danh sách
            var members = await membersQuery.ToListAsync();
            return View(members);
        }

        // 2. Xem chi tiết một member theo ID
        // Truy vấn từ database tìm member có ID tương ứng
        public async Task<IActionResult> Details(int id)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
                return NotFound();
            return View(member);
        }

        // 3. Thêm mới - GET
        // Hiển thị form tạo member mới
        public IActionResult Create()
        {
            return View();
        }

        // 3. Thêm mới - POST
        // Nhận dữ liệu từ form và lưu vào database
        [HttpPost]
        public async Task<IActionResult> Create(Member member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Thêm member vào DbSet
                    _context.Members.Add(member);
                    // Lưu thay đổi vào database (INSERT)
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Thêm mới hội viên thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm dữ liệu: " + ex.Message);
                }
            }
            return View(member);
        }

        // 4. Sửa - GET
        // Lấy thông tin member từ database và hiển thị trong form edit
        public async Task<IActionResult> Edit(int id)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
                return NotFound();
            return View(member);
        }

        // 4. Sửa - POST
        // Cập nhật thông tin member trong database
        [HttpPost]
        public async Task<IActionResult> Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra member có tồn tại không
                    var existingMember = await _context.Members.FirstOrDefaultAsync(m => m.Id == member.Id);
                    if (existingMember == null)
                        return NotFound();

                    // Cập nhật các thuộc tính
                    existingMember.Name = member.Name;
                    existingMember.Email = member.Email;
                    existingMember.Phone = member.Phone;
                    existingMember.MembershipType = member.MembershipType;
                    existingMember.Price = member.Price;

                    // Đánh dấu entity là đã sửa và lưu vào database (UPDATE)
                    _context.Members.Update(existingMember);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Cập nhật thông tin thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật dữ liệu: " + ex.Message);
                }
            }
            return View(member);
        }

        // 5. Xóa
        // Xóa member từ database theo ID
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Tìm member cần xóa từ database
                var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
                if (member != null)
                {
                    // Xóa khỏi DbSet
                    _context.Members.Remove(member);
                    // Lưu thay đổi vào database (DELETE)
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Đã xóa hội viên!";
                }
                else
                {
                    TempData["Message"] = "Không tìm thấy hội viên!";
                }
            }
            catch (DbUpdateException ex)
            {
                TempData["Message"] = "Lỗi khi xóa dữ liệu: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
