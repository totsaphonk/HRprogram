using HR.LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<EntitlementRule> EntitlementRules { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<PublicHoliday> PublicHolidays { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        public DbSet<NotificationLog> NotificationLogs { get; set; }
        public DbSet<SystemSettings> SystemSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Identity Roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "System Admin", NormalizedName = "SYSTEM ADMIN" },
                new IdentityRole { Id = "2", Name = "HR Admin", NormalizedName = "HR ADMIN" },
                new IdentityRole { Id = "3", Name = "Line Manager", NormalizedName = "LINE MANAGER" },
                new IdentityRole { Id = "4", Name = "Employee", NormalizedName = "EMPLOYEE" }
            );

            // Configure relationships
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.DepartmentNavigation)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Employee)
                .WithMany(e => e.LeaveRequests)
                .HasForeignKey(lr => lr.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.LeaveType)
                .WithMany(lt => lt.LeaveRequests)
                .HasForeignKey(lr => lr.LeaveTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EntitlementRule>()
                .HasOne(er => er.LeaveType)
                .WithMany(lt => lt.EntitlementRules)
                .HasForeignKey(er => er.LeaveTypeID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NotificationLog>()
                .HasOne(nl => nl.Template)
                .WithMany()
                .HasForeignKey(nl => nl.TemplateID)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentID = 1, Name = "Human Resources", Description = "HR Department" },
                new Department { DepartmentID = 2, Name = "IT", Description = "Information Technology" },
                new Department { DepartmentID = 3, Name = "Finance", Description = "Finance Department" }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeID = 1, FullName = "John Doe", Department = "IT", DepartmentID = 2, JoinDate = DateTime.Parse("2023-01-15"), Role = "Developer", Email = "john.doe@company.com" },
                new Employee { EmployeeID = 2, FullName = "Jane Smith", Department = "HR", DepartmentID = 1, JoinDate = DateTime.Parse("2022-06-10"), Role = "HR Manager", Email = "jane.smith@company.com" },
                new Employee { EmployeeID = 3, FullName = "Bob Johnson", Department = "Finance", DepartmentID = 3, JoinDate = DateTime.Parse("2021-03-20"), Role = "Accountant", Email = "bob.johnson@company.com" }
            );

            modelBuilder.Entity<LeaveType>().HasData(
                new LeaveType { LeaveTypeID = 1, Name = "Annual Leave", RequiresDoc = false, AllowHalfDay = true, Description = "Annual vacation leave" },
                new LeaveType { LeaveTypeID = 2, Name = "Sick Leave", RequiresDoc = true, AllowHalfDay = true, Description = "Medical leave" },
                new LeaveType { LeaveTypeID = 3, Name = "Personal Leave", RequiresDoc = false, AllowHalfDay = false, Description = "Personal time off" }
            );

            modelBuilder.Entity<EntitlementRule>().HasData(
                new EntitlementRule { RuleID = 1, LeaveTypeID = 1, Condition = "YearsOfService >= 1", EntitledDays = 14 },
                new EntitlementRule { RuleID = 2, LeaveTypeID = 2, Condition = "YearsOfService >= 0", EntitledDays = 30 },
                new EntitlementRule { RuleID = 3, LeaveTypeID = 3, Condition = "YearsOfService >= 1", EntitledDays = 5 }
            );

            // Seed Public Holidays (using static year to avoid dynamic seeding issues)
            modelBuilder.Entity<PublicHoliday>().HasData(
                new PublicHoliday { HolidayID = 1, Name = "New Year's Day", Date = new DateTime(2025, 1, 1), Description = "New Year celebration", IsRecurring = true, CreatedAt = new DateTime(2025, 1, 1) },
                new PublicHoliday { HolidayID = 2, Name = "Labour Day", Date = new DateTime(2025, 5, 1), Description = "International Workers' Day", IsRecurring = true, CreatedAt = new DateTime(2025, 1, 1) },
                new PublicHoliday { HolidayID = 3, Name = "Christmas Day", Date = new DateTime(2025, 12, 25), Description = "Christmas celebration", IsRecurring = true, CreatedAt = new DateTime(2025, 1, 1) },
                new PublicHoliday { HolidayID = 4, Name = "National Day", Date = new DateTime(2025, 8, 15), Description = "National Independence Day", IsRecurring = true, CreatedAt = new DateTime(2025, 1, 1) },
                new PublicHoliday { HolidayID = 5, Name = "Thanksgiving", Date = new DateTime(2025, 11, 28), Description = "Thanksgiving Day", IsRecurring = true, CreatedAt = new DateTime(2025, 1, 1) }
            );

            // Seed Notification Templates
            modelBuilder.Entity<NotificationTemplate>().HasData(
                new NotificationTemplate 
                { 
                    TemplateID = 1, 
                    Name = "LeaveRequestSubmitted", 
                    Type = "Email", 
                    Subject = "New Leave Request - {EmployeeName}", 
                    Body = @"<h3>New Leave Request Submitted</h3>
                            <p><strong>Employee:</strong> {EmployeeName}</p>
                            <p><strong>Leave Type:</strong> {LeaveType}</p>
                            <p><strong>From:</strong> {FromDate}</p>
                            <p><strong>To:</strong> {ToDate}</p>
                            <p><strong>Duration:</strong> {Duration} days</p>
                            <p><strong>Reason:</strong> {Reason}</p>
                            <p><strong>Request ID:</strong> {RequestId}</p>
                            <p>Please review and approve/reject this leave request.</p>",
                    Category = "LeaveRequest",
                    CreatedAt = new DateTime(2025, 1, 1)
                },
                new NotificationTemplate 
                { 
                    TemplateID = 2, 
                    Name = "LeaveRequestApproved", 
                    Type = "Email", 
                    Subject = "Leave Request Approved - {LeaveType}", 
                    Body = @"<h3>Leave Request Approved</h3>
                            <p>Dear {EmployeeName},</p>
                            <p>Your leave request has been approved!</p>
                            <p><strong>Leave Type:</strong> {LeaveType}</p>
                            <p><strong>From:</strong> {FromDate}</p>
                            <p><strong>To:</strong> {ToDate}</p>
                            <p><strong>Duration:</strong> {Duration} days</p>
                            <p><strong>Comments:</strong> {Comments}</p>
                            <p>Enjoy your time off!</p>",
                    Category = "LeaveRequest",
                    CreatedAt = new DateTime(2025, 1, 1)
                },
                new NotificationTemplate 
                { 
                    TemplateID = 3, 
                    Name = "LeaveRequestRejected", 
                    Type = "Email", 
                    Subject = "Leave Request Rejected - {LeaveType}", 
                    Body = @"<h3>Leave Request Rejected</h3>
                            <p>Dear {EmployeeName},</p>
                            <p>Unfortunately, your leave request has been rejected.</p>
                            <p><strong>Leave Type:</strong> {LeaveType}</p>
                            <p><strong>From:</strong> {FromDate}</p>
                            <p><strong>To:</strong> {ToDate}</p>
                            <p><strong>Duration:</strong> {Duration} days</p>
                            <p><strong>Reason for Rejection:</strong> {Comments}</p>
                            <p>Please contact HR if you have any questions.</p>",
                    Category = "LeaveRequest",
                    CreatedAt = new DateTime(2025, 1, 1)
                },
                new NotificationTemplate 
                { 
                    TemplateID = 4, 
                    Name = "LeaveRequestReminder", 
                    Type = "Email", 
                    Subject = "Reminder: Pending Leave Request - {EmployeeName}", 
                    Body = @"<h3>Pending Leave Request Reminder</h3>
                            <p>This is a reminder that you have a pending leave request to review:</p>
                            <p><strong>Employee:</strong> {EmployeeName}</p>
                            <p><strong>Leave Type:</strong> {LeaveType}</p>
                            <p><strong>From:</strong> {FromDate}</p>
                            <p><strong>To:</strong> {ToDate}</p>
                            <p><strong>Duration:</strong> {Duration} days</p>
                            <p><strong>Submitted:</strong> {CurrentDate}</p>
                            <p>Please review and take action on this request.</p>",
                    Category = "Reminder",
                    CreatedAt = new DateTime(2025, 1, 1)
                }
            );
        }
    }
}
