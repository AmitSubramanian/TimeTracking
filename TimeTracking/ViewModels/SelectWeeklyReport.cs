using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TimeTracking.ViewModels
{
    public class SelectWeeklyReport
    {
        [Display(Name = "Year"), Required(AllowEmptyStrings = false, ErrorMessage = "Year is required.")]
        public int Year { get; set; }
        public List<SelectListItem> YearList { get; set; }
        [Display(Name = "Month"), Required(AllowEmptyStrings = false, ErrorMessage = "Month is required.")]
        public int Month { get; set; }
        public List<SelectListItem> MonthList { get; set; }
        [Display(Name = "Work Week"), Required(AllowEmptyStrings = false, ErrorMessage = "Work Week is required.")]
        public int WorkWeek { get; set; }
        public List<SelectListItem> WorkWeekList { get; set; }
    }
}
