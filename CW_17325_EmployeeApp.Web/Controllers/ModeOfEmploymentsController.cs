using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CW_17325_EmployeeApp.Web.Data;
using CW_17325_EmployeeApp.Web.Models;

namespace CW_17325_EmployeeApp.Web.Controllers
{
    public class ModeOfEmploymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModeOfEmploymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ModeOfEmployments
        public async Task<IActionResult> Index()
        {
            return View(await _context.ModeOfEmployments.ToListAsync());
        }

        // GET: ModeOfEmployments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modeOfEmployment = await _context.ModeOfEmployments
                .FirstOrDefaultAsync(m => m.ModeOfEmploymentId == id);
            if (modeOfEmployment == null)
            {
                return NotFound();
            }

            return View(modeOfEmployment);
        }

        // GET: ModeOfEmployments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ModeOfEmployments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModeOfEmploymentId,Label")] ModeOfEmployment modeOfEmployment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modeOfEmployment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modeOfEmployment);
        }

        // GET: ModeOfEmployments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modeOfEmployment = await _context.ModeOfEmployments.FindAsync(id);
            if (modeOfEmployment == null)
            {
                return NotFound();
            }
            return View(modeOfEmployment);
        }

        // POST: ModeOfEmployments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModeOfEmploymentId,Label")] ModeOfEmployment modeOfEmployment)
        {
            if (id != modeOfEmployment.ModeOfEmploymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modeOfEmployment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModeOfEmploymentExists(modeOfEmployment.ModeOfEmploymentId))
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
            return View(modeOfEmployment);
        }

        // GET: ModeOfEmployments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modeOfEmployment = await _context.ModeOfEmployments
                .FirstOrDefaultAsync(m => m.ModeOfEmploymentId == id);
            if (modeOfEmployment == null)
            {
                return NotFound();
            }

            return View(modeOfEmployment);
        }

        // POST: ModeOfEmployments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modeOfEmployment = await _context.ModeOfEmployments.FindAsync(id);
            _context.ModeOfEmployments.Remove(modeOfEmployment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModeOfEmploymentExists(int id)
        {
            return _context.ModeOfEmployments.Any(e => e.ModeOfEmploymentId == id);
        }
    }
}
