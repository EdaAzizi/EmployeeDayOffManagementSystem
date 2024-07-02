using EmployeeDayOffManagement.Core.Dtos.LeaveAllocation;

namespace EmployeeDayOffManagement.Application.LeaveAllocation;

public interface ILeaveAllocationService
{
    Task<List<LeaveAllocationDto>> GetAllAllocations();
    Task<LeaveAllocationDto> GetAllocationById(int id);
    Task CreateAllocation(CreateLeaveAllocationDto allocation);
    Task UpdateAllocation(LeaveAllocationDto allocation);
    Task DeleteAllocation(int id);
    Task AccumulateAnnualLeave();
    Task ResetLeaveAtYearStart();
}