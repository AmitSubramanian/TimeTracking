using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.ViewModels;
using TimeTracking.Services;
using TimeTracking.Models;

namespace TimeTracking.Controllers
{
    public class ReportsController : Controller
    {
        private readonly TimeTrackingContext _timeTrackingContext;
        private readonly IUtilityService _utilityService;

        public ReportsController(TimeTrackingContext timeTrackingContext, IUtilityService utilityService)
        {
            _timeTrackingContext = timeTrackingContext;
            _utilityService = utilityService;
        }

        public IActionResult Weekly()
        {
            var view = new SelectWeeklyReport
            {
                Year = DateTime.Now.Year,
                YearList = _utilityService.GetYears(),
                Month = DateTime.Now.Month,
                MonthList = _utilityService.GetMonths(),
                WorkWeek = 1,
                WorkWeekList = _utilityService.GetWorkWeeks(DateTime.Now.Year, DateTime.Now.Month)
            };

            return View("Weekly", view);
        }

        public IActionResult WeeklyReport(int year, int month, int workWeek)
        {
            var view = _utilityService.GetWeeklyReport(year, month, workWeek);

            return View(view);
        }

        public IActionResult Employee()
        {
            var view = new SelectEmployeeReport
            {
                EmployeeId = 0,
                EmployeeList = _utilityService.GetEmployees(),
                Year = DateTime.Now.Year,
                YearList = _utilityService.GetYears(),
                Month = DateTime.Now.Month,
                MonthList = _utilityService.GetMonths(),
                WorkWeek = 1,
                WorkWeekList = _utilityService.GetWorkWeeks(DateTime.Now.Year, DateTime.Now.Month)
            };

            return View("Employee", view);
        }

        public IActionResult EmployeeReport(int employeeId, int year, int month, int workWeek)
        {
            var view = _utilityService.GetEmployeeReport(employeeId, year, month, workWeek);

            return View(view);
        }

        [NonAction]
        private SelectWeeklyReport GetSelectWeeklyReportViewModel()
        {
            return new SelectWeeklyReport
            {
                Year = DateTime.Now.Year,
                YearList = _utilityService.GetYears(),
                Month = DateTime.Now.Month,
                MonthList = _utilityService.GetMonths(),
                WorkWeek = 1,
                WorkWeekList = _utilityService.GetWorkWeeks(DateTime.Now.Year, DateTime.Now.Month)
            };
        }
    }
}
