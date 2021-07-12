using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TimeTracking.ViewModels
{
    public class AddTimeEntry
    {
        [Display(Name = "Employee"), Required(AllowEmptyStrings = false, ErrorMessage = "Employee is required.")]
        public int EmployeeId { get; set; }
        public List<SelectListItem> Employee { get; set; }
        [Display(Name = "Date Worked"), Required(AllowEmptyStrings = false, ErrorMessage = "Date Worked is required.")]
        public DateTime DateWorked { get; set; }
        [Display(Name = "Hours Worked (1 - 12)"), Range(1, 12, ErrorMessage ="Hours Worked must be between 1 and 12."), Required(AllowEmptyStrings = false, ErrorMessage = "Hours Worked is required.")]
        public int HoursWorked { get; set; }
    }
}
