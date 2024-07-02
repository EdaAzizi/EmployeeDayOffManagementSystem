using EmployeeDayOffManagement.Application.LeaveRequest;
using EmployeeDayOffManagement.Core.Dtos.LeaveRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDayOffManagement.Service.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class LeaveRequestController : ControllerBase
{
    private readonly ILeaveRequestService _leaveRequestService;
    
    public LeaveRequestController(ILeaveRequestService leaveRequestService)
    {
        _leaveRequestService = leaveRequestService;
    }
    
    [HttpGet(Name = "GetAllRequests")]
    [Authorize(Policy = "UserOnly")]
    public async Task<IActionResult> GetLeaveRequests()
    {
        var employeeId = User.Identity.Name; 
        var requests = await _leaveRequestService.GetAllRequests(employeeId);
        return Ok(requests);
    }

    [HttpGet("GetRequest/{id}")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> GetLeaveRequest(int id)
    {
        var request = await _leaveRequestService.GetRequest(id);
        return Ok(request);
    }

    [HttpPost(Name = "CreateRequest")]
    [Authorize(Policy = "UserOnly")]
    public async Task<IActionResult> CreateLeaveRequest(CreateLeaveRequestDto requestDto)
    {
        await _leaveRequestService.CreateRequest(requestDto);
        return Ok();
    }

    [HttpPut(Name = "UpdateRequest")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> UpdateLeaveRequest(LeaveRequestDto requestDto)
    {
        await _leaveRequestService.UpdateRequest(requestDto);
        return Ok();
    }

    [HttpDelete("DeleteRequest/{id}")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> DeleteLeaveRequest(int id)
    {
        await _leaveRequestService.DeleteRequest(id);
        return Ok();
    }
}