using AutoMapper;
using EmployeeDayOffManagement.Core.Dtos.Employee;
using EmployeeDayOffManagement.Core.Dtos.LeaveAllocation;
using EmployeeDayOffManagement.Core.Dtos.LeaveRequest;
using EmployeeDayOffManagement.Core.Entities;

namespace EmployeeDayOffManagement.Service.Helpers;

public class AutoMapperConfigurations : Profile
{
    public AutoMapperConfigurations() 
    {
        CreateMap<Employee, CreateEmployeeDto>().ReverseMap();
        CreateMap<CreateEmployeeDto, Employee>().ReverseMap();

        CreateMap<Employee, EmployeeDto>().ReverseMap();
        CreateMap<EmployeeDto, Employee>().ReverseMap();
        
        CreateMap<LeaveRequest, CreateLeaveRequestDto>().ReverseMap();
        CreateMap<CreateLeaveRequestDto, LeaveRequest>().ReverseMap();

        CreateMap<LeaveRequest, LeaveRequestDto>().ReverseMap();
        CreateMap<LeaveRequestDto, LeaveRequest>().ReverseMap();
        
        CreateMap<LeaveAllocation, CreateLeaveAllocationDto>().ReverseMap();
        CreateMap<CreateLeaveAllocationDto, LeaveAllocation>().ReverseMap();

        CreateMap<LeaveAllocation, LeaveAllocationDto>().ReverseMap();
        CreateMap<LeaveAllocationDto, LeaveAllocation>().ReverseMap();
    }
}