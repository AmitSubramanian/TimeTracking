using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using TimeTracking.ViewModels;

namespace TimeTracking.Services
{
    public interface IUtilityService
    {
        List<SelectListItem> GetEmployeeTypes();
        List<SelectListItem> GetEmployees();
        List<SelectListItem> GetYears();
        List<SelectListItem> GetMonths();
        List<SelectListItem> GetWorkWeeks(int year, int month);
        WeeklyReport GetWeeklyReport(int year, int month, int workWeek);
        EmployeeReport GetEmployeeReport(int employeeId, int year, int month, int workWeek);
        BreaksEarned GetBreaksEarned(char employeeType, int hoursWorked);
        DateTime GetFirstDateOfWorkWeek(int year, int month, int workWeek);
        int GetNumberofWorkWeeksInMonth(int year, int month);
    }
}
