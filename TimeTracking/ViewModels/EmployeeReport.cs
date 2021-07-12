namespace TimeTracking.ViewModels
{
    public class EmployeeReport
    {
        public string EmployeeName { get; set; }
        public char EmployeeTypeCode { get; set; }
        public string EmployeeType { get; set; }
        public int WorkWeek { get; set; }
        public int HoursWorked { get; set; }
        public BreaksEarned BreaksEarned { get; set; }
    }
}
