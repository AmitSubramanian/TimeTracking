using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeTracking.ViewModels
{
    public class DrawInputWeeklyReportForm
    {
        [Display(Name = "Month"), Required]
        public List<KeyValuePair<int, string>> Month { get; set; }
        [Display(Name = "Work Week"), Required]
        public List<int> WorkWeek { get; set; }
    }
}
