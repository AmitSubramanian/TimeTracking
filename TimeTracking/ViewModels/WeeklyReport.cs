using System.Collections.Generic;

namespace TimeTracking.ViewModels
{
    public class WeeklyReport
    {
        public int WorkWeek { get; set; }
        public List<WeeklyReportRow> WeeklyReportRows { get; set; }
    }

    public class WeeklyReportRow
    {
        public string EmployeeName { get; set; }
        public char EmployeeTypeCode { get; set; }
        public int[] HoursWorked { get; set; }
        public int TotalHoursWorked { get; set; }
        public BreaksEarned TotalBreaksEarned { get; set; }
    }
}
