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
    public class HocPhansController : Controller
    {
        private readonly Test1Context _context;

        public HocPhansController(Test1Context context)
        {
            _context = context;
        }

        // GET: HocPhans
        public async Task<IActionResult> Index()
        {
            return View(await _context.HocPhans.ToListAsync());
        }

        // GET: HocPhans/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hocPhan = await _context.HocPhans
                .FirstOrDefaultAsync(m => m.MaHp == id);
            if (hocPhan == null)
            {
                return NotFound();
            }

            return View(hocPhan);
        }

        // GET: HocPhans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HocPhans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHp,TenHp,SoTinChi")] HocPhan hocPhan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hocPhan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hocPhan);
        }

        // GET: HocPhans/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hocPhan = await _context.HocPhans.FindAsync(id);
            if (hocPhan == null)
            {
                return NotFound();
            }
            return View(hocPhan);
        }

        // POST: HocPhans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaHp,TenHp,SoTinChi")] HocPhan hocPhan)
        {
            if (id != hocPhan.MaHp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hocPhan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HocPhanExists(hocPhan.MaHp))
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
            return View(hocPhan);
        }

        // GET: HocPhans/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hocPhan = await _context.HocPhans
                .FirstOrDefaultAsync(m => m.MaHp == id);
            if (hocPhan == null)
            {
                return NotFound();
            }

            return View(hocPhan);
        }

        // POST: HocPhans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var hocPhan = await _context.HocPhans.FindAsync(id);
            if (hocPhan != null)
            {
                _context.HocPhans.Remove(hocPhan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HocPhanExists(string id)
        {
            return _context.HocPhans.Any(e => e.MaHp == id);
        }
    }
}
