using EmployeeDayOffManagement.Application.Employee;
using EmployeeDayOffManagement.Core.Dtos.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDayOffManagement.Service.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet(Name = "GetEmployees")]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await _employeeService.GetAllEmployees();

        return Ok(employees);
    }
    
    [HttpGet("GetEmployee/{id}")]
    public async Task<IActionResult> GetEmployee(string id)
    {
        var employee = await _employeeService.GetEmployee(id);

        return Ok(employee);
    }

    [HttpPost(Name = "AddEmployee")]
    public async Task<IActionResult> AddEmployee(CreateEmployeeDto employeeDto)
    {
        await _employeeService.AddEmployee(employeeDto);

        return Ok();
    }
    
    [HttpPut(Name = "UpdateEmployee")]
    public async Task<IActionResult> UpdateEmployee(EmployeeDto employeeDto)
    {
        await _employeeService.UpdateEmployee(employeeDto);

        return Ok();
    }

    [HttpDelete("DeleteEmployee/{id}")]
    public async Task<IActionResult> DeleteEmployee(string id)
    {
        await _employeeService.DeleteEmployee(id);

        return Ok();
    }
}