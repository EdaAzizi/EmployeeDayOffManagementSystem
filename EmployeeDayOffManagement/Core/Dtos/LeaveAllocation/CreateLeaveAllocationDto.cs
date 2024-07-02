namespace EmployeeDayOffManagement.Core.Dtos.LeaveAllocation;

public class CreateLeaveAllocationDto
{
    public int NumberOfDays { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public string EmployeeId { get; set; }
    public int LeaveTypeId { get; set; }
    public int Period { get; set; }
}