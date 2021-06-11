using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW_17325_EmployeeApp.Web.ViewModels
{
    public class EmployeeAttendance
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }

        public double Duration { get; set; }
        public double TotalHours { get; set; }
    }
}
