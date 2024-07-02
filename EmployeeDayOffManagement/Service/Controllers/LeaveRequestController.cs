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
    
    [HttpGet]
    public async Task<IActionResult> GetAllRequests()
    {
        var employeeId = User.Identity.Name; 
        var requests = await _leaveRequestService.GetAllRequests(employeeId);
        return Ok(requests);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRequest(int id)
    {
        var request = await _leaveRequestService.GetRequest(id);
        return Ok(request);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRequest(CreateLeaveRequestDto requestDto)
    {
        await _leaveRequestService.CreateRequest(requestDto);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRequest(LeaveRequestDto requestDto)
    {
        await _leaveRequestService.UpdateRequest(requestDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRequest(int id)
    {
        await _leaveRequestService.DeleteRequest(id);
        return Ok();
    }
}