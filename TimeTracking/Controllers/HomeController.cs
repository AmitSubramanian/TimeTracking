using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.ViewModels;
using TimeTracking.Services;
using TimeTracking.Models;

namespace TimeTracking.Controllers
{
    public class HomeController : Controller
    {
        private readonly TimeTrackingContext _timeTrackingContext;
        private readonly IUtilityService _utilityService;

        public HomeController(TimeTrackingContext timeTrackingContext, IUtilityService utilityService)
        {
            _timeTrackingContext = timeTrackingContext;
            _utilityService = utilityService;
        }

        public ActionResult<List<EmployeeDetails>> Index()
        {
            var result = _timeTrackingContext.Employees
                .Select(x => new EmployeeDetails() {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    EmployeeTypeDescription = x.EmployeeType.Description
                })
                .OrderBy(o => o.Id)
                .ToList();

            if (TempData["Message"] != null)
            {
                ViewData["Message"] = TempData["Message"];
            }

            return View(result);
        }

        public IActionResult Add()
        {
            var view = new AddEmployee();
            view.EmployeeType = _utilityService.GetEmployeeTypes();

            return View(view);
        }

        [HttpPost]
        public IActionResult Add(AddEmployee emp)
        {
            var employee = new Employee()
            {
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                EmployeeTypeCode = emp.EmployeeTypeCode
            };

            _timeTrackingContext.Employees.Add(employee);
            _timeTrackingContext.SaveChanges();

            TempData["Message"] = "Employee successfully added.";

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
