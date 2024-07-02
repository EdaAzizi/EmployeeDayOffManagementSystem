using System.ComponentModel.DataAnnotations;

namespace EmployeeDayOffManagement.Core.Entities;

public class LeaveType
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int DefaultDays { get; set; }
    public DateTime DateCreated { get; set; }
    public ICollection<LeaveRequest> LeaveRequests { get; set; }
    public ICollection<LeaveAllocation> LeaveAllocations { get; set; }
}