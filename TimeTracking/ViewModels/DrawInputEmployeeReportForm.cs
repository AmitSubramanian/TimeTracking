using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeTracking.ViewModels
{
    public class DrawInputEmployeeReportForm
    {
        [Display(Name = "Employee"), Required]
        public List<KeyValuePair<int, string>> Employee { get; set; }
        [Display(Name = "Month"), Required]
        public List<KeyValuePair<int, string>> Month { get; set; }
        [Display(Name = "Work Week"), Required]
        public List<int> WorkWeek { get; set; }
    }
}
