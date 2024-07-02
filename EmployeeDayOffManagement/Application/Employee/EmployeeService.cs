using AutoMapper;
using EmployeeDayOffManagement.Application.Interfaces;
using EmployeeDayOffManagement.Core.Dtos.Employee;
using EmployeeDayOffManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDayOffManagement.Application.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> GetAllEmployees()
        {
            var employees = await _unitOfWork.Repository<Core.Entities.Employee>().GetAll().ToListAsync();
            var employeeDtos = _mapper.Map<List<EmployeeDto>>(employees);
            return employeeDtos;
        }
        public async Task<EmployeeDto> GetEmployee(string id)
        {
            var employee = await _unitOfWork.Repository<Core.Entities.Employee>().GetById(x => x.Id == id).FirstOrDefaultAsync();
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return employeeDto;
        }

        public async Task AddEmployee(CreateEmployeeDto employee)
        {
            var employeeToCreate = _mapper.Map<Core.Entities.Employee>(employee);
            _unitOfWork.Repository<Core.Entities.Employee>().Create(employeeToCreate);
            _unitOfWork.Complete();
        }

        public async Task UpdateEmployee(EmployeeDto employee)
        {
            var employeeToUpdate = await GetEmployee(employee.Id);

            if (employee != null)
            {
                employeeToUpdate.Firstname = employee.Firstname;
                employeeToUpdate.Lastname = employee.Lastname;
                employeeToUpdate.ReportsTo = employee.ReportsTo;
                employeeToUpdate.Position = employee.Position;
                employeeToUpdate.DateOfBirth = employee.DateOfBirth;
            }

            var employeeUpdated = _mapper.Map<Core.Entities.Employee>(employee);
            _unitOfWork.Repository<Core.Entities.Employee>().Update(employeeUpdated);
            _unitOfWork.Complete();

        }

        public async Task DeleteEmployee(string id)
        {
            var employee = await _unitOfWork.Repository<Core.Entities.Employee>().GetById(e => e.Id == id).FirstOrDefaultAsync();
            if (employee != null)
            {
                _unitOfWork.Repository<Core.Entities.Employee>().Delete(employee);
                _unitOfWork.Complete();
            }
        }
    }
}
