namespace EmployeeDayOffManagement.Core.Entities;

public class Employee
{
    public string Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Position { get; set; }
    public string ReportsTo { get; set; } 
    public DateTime DateOfBirth { get; set; }
    public DateTime DateJoined { get; set; } = DateTime.Now;
    public ICollection<LeaveRequest> LeaveRequests { get; set; }
    public ICollection<LeaveAllocation> LeaveAllocations { get; set; }
    public ICollection<Employee> Subordinates { get; set; }
}