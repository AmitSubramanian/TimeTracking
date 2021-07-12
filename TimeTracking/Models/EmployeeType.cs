using System;
using System.ComponentModel.DataAnnotations;

namespace TimeTracking.Models
{
    public class EmployeeType
    {
        [Key]
        public char Code { get; set; }
        [Required, MaxLength(50)]
        public string Description { get; set; }
    }
}
