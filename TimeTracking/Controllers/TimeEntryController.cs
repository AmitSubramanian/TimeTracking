using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTracking.ViewModels;
using TimeTracking.Services;
using TimeTracking.Models;
using Microsoft.AspNetCore.Mvc;

namespace TimeTracking.Controllers
{
    public class TimeEntryController : Controller
    {
        private readonly TimeTrackingContext _timeTrackingContext;
        private readonly IUtilityService _utilityService;

        public TimeEntryController(TimeTrackingContext timeTrackingContext, IUtilityService utilityService)
        {
            _timeTrackingContext = timeTrackingContext;
            _utilityService = utilityService;
        }

        public IActionResult Index()
        {
            var view = new AddTimeEntry();
            view.Employee = _utilityService.GetEmployees();
            view.DateWorked = DateTime.Now;

            if (TempData["Message"] != null)
            {
                ViewData["Message"] = TempData["Message"];
            }

            return View(view);
        }

        [HttpPost]
        public IActionResult Add(AddTimeEntry te)
        {
            bool entryExists = (_timeTrackingContext.TimeEntries
                .Where(w => w.EmployeeId == te.EmployeeId && w.DateWorked == te.DateWorked)
                .Select(x => 'x')
                .Count()) > 0;

            if (entryExists)
            {
                TempData["Message"] = "The time entry already exists.";
            }
            else
            {
                var timeEntry = new TimeEntry()
                {
                    EmployeeId = te.EmployeeId,
                    DateWorked = te.DateWorked,
                    HoursWorked = te.HoursWorked
                };

                _timeTrackingContext.TimeEntries.Add(timeEntry);
                _timeTrackingContext.SaveChanges();

                TempData["Message"] = "Time Entry successfully added.";
            }

            return RedirectToAction("Index");
        }
    }
}
