using EmployeeDayOffManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDayOffManagement.Infrastructure;

public class LmsDbContext : DbContext
{
    public LmsDbContext(DbContextOptions<LmsDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Employee> Employees { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<LeaveAllocation> LeaveAllocations { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data for LeaveTypes
        modelBuilder.Entity<LeaveType>().HasData(
            new LeaveType { Id = 1, Name = "Annual", DefaultDays = 0, DateCreated = DateTime.Now },
            new LeaveType { Id = 2, Name = "Sick", DefaultDays = 20, DateCreated = DateTime.Now },
            new LeaveType { Id = 3, Name = "Replacement", DefaultDays = 0, DateCreated = DateTime.Now },
            new LeaveType { Id = 4, Name = "Unpaid", DefaultDays = 10, DateCreated = DateTime.Now }
        );
        
        // Configure Employee - LeaveRequest relationship
        modelBuilder.Entity<LeaveRequest>()
            .HasOne(lr => lr.RequestingEmployee)
            .WithMany(e => e.LeaveRequests)
            .HasForeignKey(lr => lr.RequestingEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Employee - LeaveAllocation relationship
        modelBuilder.Entity<LeaveAllocation>()
            .HasOne(la => la.Employee)
            .WithMany(e => e.LeaveAllocations)
            .HasForeignKey(la => la.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure LeaveType - LeaveRequest relationship
        modelBuilder.Entity<LeaveRequest>()
            .HasOne(lr => lr.LeaveType)
            .WithMany(lt => lt.LeaveRequests)
            .HasForeignKey(lr => lr.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure LeaveType - LeaveAllocation relationship
        modelBuilder.Entity<LeaveAllocation>()
            .HasOne(la => la.LeaveType)
            .WithMany(lt => lt.LeaveAllocations)
            .HasForeignKey(la => la.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure self-referencing relationship for Employee
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Subordinates)
            .WithOne()
            .HasForeignKey(e => e.ReportsTo)
            .OnDelete(DeleteBehavior.Restrict);
    }
}