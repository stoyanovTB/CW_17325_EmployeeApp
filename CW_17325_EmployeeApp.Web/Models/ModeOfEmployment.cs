using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW_17325_EmployeeApp.Web.Models
{
    public class ModeOfEmployment
    {
        public int ModeOfEmploymentId { get; set; }
        public string Label { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
