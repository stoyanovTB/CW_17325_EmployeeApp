using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CW_17325_EmployeeApp.Web.Data;
using CW_17325_EmployeeApp.Web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace CW_17325_EmployeeApp.Web.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeesController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Employees.Include(e => e.ApplicationUser).Include(e => e.ModeOfEmployment);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<List<Employee>> GetEmployeeList()
        {

            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            //Method Syntax
            var employees = _context.Employees
                                    .Include(s => s.ApplicationUser)
                                    .Include(s => s.ModeOfEmployment);

            if (userId == 1)
            {
                return await employees.ToListAsync();
            }
            else
            {
                return await employees.Where(x => x.ApplicationUserId == userId).ToListAsync();

                //SELECT * FROM Employees WHERE ApplicationUserId = whoever is logged in's Id 
            }
        }

        public async Task<IActionResult> CheckIn(int Id)
        {

            var employee = await _context.Employees
                                    .Where(x => x.EmployeeId == Id).FirstOrDefaultAsync();

            if (employee.IsCheckedIn == "No" || employee.IsCheckedIn == null)
            {
                var attendance = new Attendance();
                attendance.EmployeeId = Id;
                attendance.TimeIn = DateTime.Now;
                _context.Add(attendance);
                employee.IsCheckedIn = "Yes";

            }
            else
            {
                var result = await _context.Attendances
                                       .Include(x => x.Employee)
                                       .FirstOrDefaultAsync(x => x.EmployeeId == Id &&
                                       x.TimeIn != DateTime.MinValue && x.TimeOut == DateTime.MinValue);

                if (result != null)
                {
                    var attendance = await _context.Attendances.FirstOrDefaultAsync(x => x.AttendanceId == result.AttendanceId);
                    attendance.TimeOut = DateTime.Now;
                    employee.IsCheckedIn = "No";
                    _context.Entry(attendance).State = EntityState.Modified;
                }
            }

            _context.Entry(employee).State = EntityState.Modified;

            await _context.SaveChangesAsync(); //unit of work 

            return RedirectToAction(nameof(Index));
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.ApplicationUser)
                .Include(e => e.ModeOfEmployment)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ModeOfEmploymentId"] = new SelectList(_context.ModeOfEmployments, "ModeOfEmploymentId", "ModeOfEmploymentId");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,DateOfBirth,Mobile,Email,IsCheckedIn,ApplicationUserId,ModeOfEmploymentId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", employee.ApplicationUserId);
            ViewData["ModeOfEmploymentId"] = new SelectList(_context.ModeOfEmployments, "ModeOfEmploymentId", "ModeOfEmploymentId", employee.ModeOfEmploymentId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", employee.ApplicationUserId);
            ViewData["ModeOfEmploymentId"] = new SelectList(_context.ModeOfEmployments, "ModeOfEmploymentId", "ModeOfEmploymentId", employee.ModeOfEmploymentId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,DateOfBirth,Mobile,Email,IsCheckedIn,ApplicationUserId,ModeOfEmploymentId")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", employee.ApplicationUserId);
            ViewData["ModeOfEmploymentId"] = new SelectList(_context.ModeOfEmployments, "ModeOfEmploymentId", "ModeOfEmploymentId", employee.ModeOfEmploymentId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.ApplicationUser)
                .Include(e => e.ModeOfEmployment)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
