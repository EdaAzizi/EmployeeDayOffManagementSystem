namespace EmployeeDayOffManagement.Core.Dtos.LeaveRequest;

public class LeaveRequestDto
{
    public int Id { get; set; }
    public string EmployeeId { get; set; }
    public int LeaveTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsApproved { get; set; }
    public string LeadId { get; set; }
}