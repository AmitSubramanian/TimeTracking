using System.ComponentModel.DataAnnotations;

namespace TimeTracking.ViewModels
{
    public class EmployeeDetails
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeTypeDescription { get; set; }
    }
}
