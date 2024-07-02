namespace EmployeeDayOffManagement.Core.Dtos.LeaveRequest;

public class CreateLeaveRequestDto
{
    public string EmployeeId { get; set; }
    public int LeaveTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string LeadId { get; set; }
}