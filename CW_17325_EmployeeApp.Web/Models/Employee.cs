using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW_17325_EmployeeApp.Web.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }
            
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Mobile { get; set; }
        public string Email{ get; set; }
        public string IsCheckedIn { get; set; }
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ModeOfEmploymentId { get; set; }
        public ModeOfEmployment ModeOfEmployment { get; set; }
        public ICollection<Attendance> Attendances { get; set; }


    }
}
