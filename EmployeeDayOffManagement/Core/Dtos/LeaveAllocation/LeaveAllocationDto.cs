namespace EmployeeDayOffManagement.Core.Dtos.LeaveAllocation;

public class LeaveAllocationDto
{
    public int Id { get; set; }
    public int NumberOfDays { get; set; }
    public DateTime DateCreated { get; set; }
    public string EmployeeId { get; set; }
    public int LeaveTypeId { get; set; }
    public int Period { get; set; }
}