using EmployeeDayOffManagement.Core.Dtos.LeaveRequest;

namespace EmployeeDayOffManagement.Application.LeaveRequest;

public interface ILeaveRequestService
{
    Task<List<LeaveRequestDto>> GetAllRequests(string employeeId);
    Task<LeaveRequestDto> GetRequest(int id);
    Task CreateRequest(CreateLeaveRequestDto requestDto);
    Task UpdateRequest(LeaveRequestDto requestDto);
    Task DeleteRequest(int id);
}