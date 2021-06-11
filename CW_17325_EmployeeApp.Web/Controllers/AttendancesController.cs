using CW_17325_EmployeeApp.Web.Data;
using CW_17325_EmployeeApp.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW_17325_EmployeeApp.Web.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var employeeAttendances = new List<EmployeeAttendance>();

            var attendance = _context.Attendances
                .Include(x => x.Employee)
                .ToList();

            //automapper is prefered
            double totalHours = 0;

            foreach (var item in attendance)
            {
                var model = new EmployeeAttendance();
                model.EmployeeId = item.EmployeeId;
                model.FirstName = item.Employee.FirstName;
                model.LastName = item.Employee.LastName;
                model.TimeIn = item.TimeIn;
                model.TimeOut = item.TimeOut;
                model.Duration = (item.TimeOut - item.TimeIn).TotalHours;
                totalHours = totalHours + model.Duration;
                model.TotalHours = totalHours;
                employeeAttendances.Add(model);
            }
            return View(employeeAttendances);
        }
    }
}
