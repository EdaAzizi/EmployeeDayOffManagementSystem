using EmployeeDayOffManagement.Application.Interfaces;
using EmployeeDayOffManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDayOffManagement.Application.LeaveAccumulator;

public class LeaveAccumulatorService : BackgroundService
{
    private readonly ILogger<LeaveAccumulatorService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public LeaveAccumulatorService(IUnitOfWork unitOfWork, ILogger<LeaveAccumulatorService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await AccumulateAnnualLeave();

            await ResetLeaveAtYearStart();

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    public async Task AccumulateAnnualLeave()
    {
        var currentYear = DateTime.Now.Year;
        var employees = await _unitOfWork.Repository<Core.Entities.Employee>().GetAll().ToListAsync();
        var leaveType = await _unitOfWork.Repository<LeaveType>().GetByCondition(l => l.Name == "Annual").FirstOrDefaultAsync();

        foreach (var employee in employees)
        {
            var allocation = await _unitOfWork.Repository<Core.Entities.LeaveAllocation>()
                .GetByCondition(a => a.EmployeeId == employee.Id && a.LeaveTypeId == leaveType.Id && a.Period == currentYear)
                .FirstOrDefaultAsync();

            if (allocation == null)
            {
                allocation = new Core.Entities.LeaveAllocation
                {
                    EmployeeId = employee.Id,
                    LeaveTypeId = leaveType.Id,
                    NumberOfDays = 2, 
                    DateCreated = DateTime.Now,
                    Period = currentYear
                };
                _unitOfWork.Repository<Core.Entities.LeaveAllocation>().Create(allocation);
            }
            else
            {
                allocation.NumberOfDays += 2;
                _unitOfWork.Repository<Core.Entities.LeaveAllocation>().Update(allocation);
            }
        }
        
        _unitOfWork.Complete();
    }


    public async Task ResetLeaveAtYearStart()
    {
        var currentYear = DateTime.Now.Year;
        var previousYear = currentYear - 1;
    
        var previousYearAllocations = await _unitOfWork.Repository<Core.Entities.LeaveAllocation>()
            .GetByCondition(a => a.Period == previousYear)
            .ToListAsync();

        var leaveType = await _unitOfWork.Repository<LeaveType>().GetByCondition(l => l.Name == "Annual").FirstOrDefaultAsync();

        if (leaveType != null)
        {
            foreach (var allocation in previousYearAllocations)
            {
                allocation.NumberOfDays = 0; 

                _unitOfWork.Repository<Core.Entities.LeaveAllocation>().Update(allocation);
            }
            
            _unitOfWork.Complete();
        }
    }

}