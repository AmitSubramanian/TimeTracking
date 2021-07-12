using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TimeTracking.ViewModels
{
    public class AddEmployee
    {
        [Display(Name = "First Name"), MaxLength(100), Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name"), MaxLength(100), Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }
        [Display(Name = "Employee Type"), Required(AllowEmptyStrings = false, ErrorMessage = "Employee Type is required.")]
        public char EmployeeTypeCode { get; set; }
        public List<SelectListItem> EmployeeType { get; set; }
    }
}
