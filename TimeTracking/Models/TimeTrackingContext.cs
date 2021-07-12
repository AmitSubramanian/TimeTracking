using Microsoft.EntityFrameworkCore;
using System;

namespace TimeTracking.Models
{
    public class TimeTrackingContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        public TimeTrackingContext()
        { }

        public TimeTrackingContext(DbContextOptions<TimeTrackingContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=timetracking.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Configure Keys
            modelBuilder.Entity<TimeEntry>().HasKey(c => new { c.EmployeeId, c.DateWorked });
            #endregion

            #region EmployeeType Seed
            modelBuilder.Entity<EmployeeType>().HasData(new EmployeeType() { Code = 'D', Description = "Dayshift"} );
            modelBuilder.Entity<EmployeeType>().HasData(new EmployeeType() { Code = 'N', Description = "Nightshift" } );
            #endregion

            #region Employee Seed
            modelBuilder.Entity<Employee>().HasData(new Employee { Id = 1, FirstName = "Ann", LastName = "Smith", EmployeeTypeCode = 'D'});
            modelBuilder.Entity<Employee>().HasData(new Employee { Id = 2, FirstName = "Ben", LastName = "Doe", EmployeeTypeCode = 'D' });
            #endregion
        }
    }
}
