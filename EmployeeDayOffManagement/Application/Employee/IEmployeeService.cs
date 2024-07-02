using EmployeeDayOffManagement.Core.Dtos.Employee;

namespace EmployeeDayOffManagement.Application.Employee;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> GetAllEmployees();
    Task<EmployeeDto> GetEmployee(string id);
    Task AddEmployee(CreateEmployeeDto employee);
    Task UpdateEmployee(EmployeeDto employee);
    Task DeleteEmployee(string id);

}