using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using TimeTracking.ViewModels;
using TimeTracking.Models;

namespace TimeTracking.Services
{
    public class UtilityService : IUtilityService 
    {
        private readonly TimeTrackingContext _timeTrackingContext;

        public UtilityService(TimeTrackingContext timeTrackingContext)
        {
            _timeTrackingContext = timeTrackingContext;
        }

        public List<SelectListItem> GetEmployeeTypes()
        {
            var result = new List<SelectListItem>();

            foreach (var et in _timeTrackingContext.EmployeeTypes.Select(x => x).OrderBy(o => o.Code).ToList())
            {
                result.Add(new SelectListItem { Value = et.Code.ToString(), Text = et.Description });
            }

            return result;
        }

        public List<SelectListItem> GetEmployees()
        {
            var result = new List<SelectListItem>();

            foreach (var e in _timeTrackingContext.Employees.Select(x => x).OrderBy(o => o.FirstName + " " + o.LastName).ToList())
            {
                result.Add(new SelectListItem { Value = e.Id.ToString(), Text = e.FirstName + " " + e.LastName });
            }

            return result;
        }

        public List<SelectListItem> GetYears()
        {
            string previousYear = (DateTime.Now.Year - 1).ToString();
            string currentYear = DateTime.Now.Year.ToString();
            string nextYear = (DateTime.Now.Year + 1).ToString();

            return new List<SelectListItem>()
            {
                new SelectListItem { Value = previousYear, Text = previousYear },
                new SelectListItem { Value = currentYear, Text = currentYear, Selected = true },
                new SelectListItem { Value = nextYear, Text = nextYear }
            };
        }

        public List<SelectListItem> GetMonths()
        {
            return DateTimeFormatInfo.InvariantInfo.MonthNames
                .Select((monthName, index) => new SelectListItem
                {
                    Value = (index + 1).ToString(),
                    Text = monthName,
                    Selected = (DateTime.Now.Month == index + 1)
                })
                .ToList();
        }

        public List<SelectListItem> GetWorkWeeks(int year, int month)
        {
            int numWorkWeeks = GetNumberofWorkWeeksInMonth(year, month);

            var lst = new List<SelectListItem>();

            for (int i = 1; i <= numWorkWeeks; i++)
            {
                lst.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString(), Selected = (i == 1) });
            }

            return lst;
        }

        public WeeklyReport GetWeeklyReport(int year, int month, int workWeek)
        {
            var wr = new WeeklyReport();
            wr.WorkWeek = workWeek;
            wr.WeeklyReportRows = new List<WeeklyReportRow>();

            var firstDateOfWorkWeek = GetFirstDateOfWorkWeek(year, month, workWeek);

            // Hit the database just once.  Get time entries for all employees in the work week
            var timeEntries = _timeTrackingContext.TimeEntries
                .Where(w => w.DateWorked >= firstDateOfWorkWeek && w.DateWorked < firstDateOfWorkWeek.AddDays(7))
                .OrderBy(o => o.Employee.FirstName).ThenBy(o => o.EmployeeId).ThenBy(o => o.DateWorked)
                .Select(x => new {
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName,
                    EmployeeTypeCode = x.Employee.EmployeeTypeCode,
                    DayOfWorkWeek = (x.DateWorked - firstDateOfWorkWeek).Days,
                    HoursWorked = x.HoursWorked
                });

            if (timeEntries.Count() == 0)
                return wr;

            // Transform timeEntries to WeeklyReport, in this way
            //    - Iterate over timeEntries
            //      -- set WeeklyReport properties (e.g. EmployeeName) to timeEntries property
            //      -- when the timeEntries of one employee have been iterated thru, i.e. next employee rows begin,
            //         calculate WeeklyReport's HoursWorked & BreaksEarned properties.
            int empId = -1;
            WeeklyReportRow wrr = null;

            foreach (var te in timeEntries)
            {
                if (empId != te.EmployeeId)
                {
                    // Calculate total hours worked and breaks earned for current employee
                    if (wrr != null)
                    {
                        wrr.TotalHoursWorked = wrr.HoursWorked.Sum();
                        wrr.TotalBreaksEarned = GetBreaksEarned(wrr.EmployeeTypeCode, wrr.TotalHoursWorked);
                    }

                    // Move on to next employee
                    empId = te.EmployeeId;

                    wrr = new WeeklyReportRow();
                    wrr.EmployeeName = te.EmployeeName;
                    wrr.EmployeeTypeCode = te.EmployeeTypeCode;
                    wrr.HoursWorked = new int[7];

                    wr.WeeklyReportRows.Add(wrr);
                }

                wrr.HoursWorked[te.DayOfWorkWeek] = te.HoursWorked;
            }

            // Calculate total hours worked and breaks earned for last employee
            if (wrr != null)
            {
                wrr.TotalHoursWorked = wrr.HoursWorked.Sum();
                wrr.TotalBreaksEarned = GetBreaksEarned(wrr.EmployeeTypeCode, wrr.TotalHoursWorked);
            }

            return wr;
        }

        public EmployeeReport GetEmployeeReport(int employeeId, int year, int month, int workWeek)
        {
            var firstDateOfWorkWeek = GetFirstDateOfWorkWeek(year, month, workWeek);

            var er = _timeTrackingContext.Employees
                .Where(w => w.Id == employeeId)
                .Select(x => new EmployeeReport
                {
                    EmployeeName = x.FirstName + " " + x.LastName,
                    EmployeeTypeCode = x.EmployeeTypeCode,
                    EmployeeType = x.EmployeeType.Description
                })
                .SingleOrDefault();

            er.WorkWeek = workWeek;

            er.HoursWorked = _timeTrackingContext.TimeEntries
                .Where(w => w.EmployeeId == employeeId &&
                            w.DateWorked >= firstDateOfWorkWeek && w.DateWorked < firstDateOfWorkWeek.AddDays(7))
                .Select(x => x.HoursWorked)
                .Sum();

            er.BreaksEarned = GetBreaksEarned(er.EmployeeTypeCode, er.HoursWorked);

            return er;
        }

        public BreaksEarned GetBreaksEarned(char employeeType, int hoursWorked)
        {
            int breaksEarned = 0;

            switch (employeeType)
            {
                case 'D':
                    breaksEarned = hoursWorked * 10 + (hoursWorked / 4) * 20;
                    break;
                case 'N':
                    breaksEarned = hoursWorked * 15 + (hoursWorked / 4) * 30 + (hoursWorked / 12) * 40;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            int breakHoursEarned = breaksEarned / 60;
            int breakMinutesEarned = breaksEarned - (breakHoursEarned * 60);

            return new BreaksEarned { Hours = breakHoursEarned, Minutes = breakMinutesEarned };
        }

        public DateTime GetFirstDateOfWorkWeek(int year, int month, int workWeek)
        {
            int dayOnFirstMondayOfMonth = GetDayOnFirstMonday(year, month);
            int dayOnFirstDateOfWorkWeek = dayOnFirstMondayOfMonth + (workWeek - 1) * 7;

            if (dayOnFirstDateOfWorkWeek > DateTime.DaysInMonth(year, month))
                throw new InvalidOperationException();

            return new DateTime(year, month, dayOnFirstDateOfWorkWeek);
        }

        public int GetNumberofWorkWeeksInMonth(int year, int month)
        {
            int dayOnFirstMondayOfMonth = GetDayOnFirstMonday(year, month);
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int numberOfMondaysInMonth = 1 + (daysInMonth - dayOnFirstMondayOfMonth) / 7;

            return numberOfMondaysInMonth;
        }

        private int GetDayOnFirstMonday(int year, int month)
        {
            DateTime firstOfMonth = new DateTime(year, month, 1);

            return 1 + (DayOfWeek.Monday + 7 - firstOfMonth.DayOfWeek) % 7;
        }
    }
}
