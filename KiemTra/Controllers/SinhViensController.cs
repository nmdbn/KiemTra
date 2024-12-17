using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KiemTra.Models;

namespace KiemTra.Controllers
{
    public class SinhViensController : Controller
    {
        private readonly Test1Context _context;

        public SinhViensController(Test1Context context)
        {
            _context = context;
        }

        // GET: SinhViens
        public async Task<IActionResult> Index()
        {
            var test1Context = _context.SinhViens.Include(s => s.MaNganhNavigation);
            return View(await test1Context.ToListAsync());
        }

        // GET: SinhViens/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhViens
                .Include(s => s.MaNganhNavigation)
                .FirstOrDefaultAsync(m => m.MaSv == id);
            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            //Thay đổi đường dẫn theo cấu hình của bạn
            var savePath = Path.Combine("wwwroot/Content/images", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/Content/images/" + image.FileName; // Trả về đường dẫn tương đối
        }

        // GET: SinhViens/Create
        public IActionResult Create()
        {
            ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "MaNganh");
            return View();
        }

        // POST: SinhViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSv,HoTen,GioiTinh,NgaySinh,Hinh,MaNganh")] SinhVien sinhVien, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage
                    sinhVien.Hinh= await SaveImage(imageUrl);
                }

                await _context.AddAsync(sinhVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "MaNganh", sinhVien.MaNganh);
            return View(sinhVien);
        }

        // GET: SinhViens/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "MaNganh", sinhVien.MaNganh);
            return View(sinhVien);
        }

        // POST: SinhViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaSv,HoTen,GioiTinh,NgaySinh,Hinh,MaNganh")] SinhVien sinhVien, IFormFile imageUrl)
        {
            ModelState.Remove("ImageUrl");

            if (id != sinhVien.MaSv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSV = await _context.SinhViens.AsNoTracking().FirstOrDefaultAsync(b => b.MaSv == id);
                    if (existingSV == null)
                    {
                        return NotFound();
                    }

                    if (imageUrl == null)
                    {
                        // Giữ lại hình ảnh cũ
                        sinhVien.Hinh = existingSV.Hinh;
                    }
                    else
                    {
                        // Lưu hình ảnh mới
                        sinhVien.Hinh = await SaveImage(imageUrl);
                    }

                    // Cập nhật thông tin sinh viên
                    _context.Update(sinhVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SinhVienExists(sinhVien.MaSv))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi, giữ lại dữ liệu nhập
            ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "MaNganh", sinhVien.MaNganh);
            return View(sinhVien);
        }



        // GET: SinhViens/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhViens
                .Include(s => s.MaNganhNavigation)
                .FirstOrDefaultAsync(m => m.MaSv == id);
            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        // POST: SinhViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien != null)
            {
                _context.SinhViens.Remove(sinhVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SinhVienExists(string id)
        {
            return _context.SinhViens.Any(e => e.MaSv == id);
        }
    }
}
