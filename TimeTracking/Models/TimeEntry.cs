using System;

namespace TimeTracking.Models
{
    public class TimeEntry
    {
        public DateTime DateWorked { get; set; }
        public int HoursWorked { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
