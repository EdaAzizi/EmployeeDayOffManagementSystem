using EmployeeDayOffManagement.Application.LeaveAllocation;
using EmployeeDayOffManagement.Core.Dtos.LeaveAllocation;
using EmployeeDayOffManagement.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDayOffManagement.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class LeaveAllocationController : ControllerBase
{
    private readonly ILeaveAllocationService _leaveAllocationService;

    public LeaveAllocationController(ILeaveAllocationService leaveAllocationService)
    {
        _leaveAllocationService = leaveAllocationService;
    }

    [HttpGet(Name = "GetAllocations")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<List<LeaveAllocationDto>>> GetLeaveAllocations()
    {
        var allocations = await _leaveAllocationService.GetAllAllocations();
        return Ok(allocations);
    }

    [HttpGet("GetAllocation/{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<LeaveAllocationDto>> GetLeaveAllocation(int id)
    {
        var allocation = await _leaveAllocationService.GetAllocationById(id);
        return Ok(allocation);
    }

    [HttpPost(Name = "CreateAllocation")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult> CreateLeaveAllocation(CreateLeaveAllocationDto allocation)
    {
        await _leaveAllocationService.CreateAllocation(allocation);
        return Ok();
    }

    [HttpPut(Name = "UpdateAllocation")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult> UpdateLeaveAllocation(LeaveAllocationDto allocation)
    {
        await _leaveAllocationService.UpdateAllocation(allocation);
        return Ok();
    }

    [HttpDelete("DeleteAllocation/{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult> DeleteLeaveAllocation(int id)
    {
        await _leaveAllocationService.DeleteAllocation(id);
        return NoContent();
    }
}