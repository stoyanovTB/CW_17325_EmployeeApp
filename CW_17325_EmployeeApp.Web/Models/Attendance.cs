using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW_17325_EmployeeApp.Web.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
