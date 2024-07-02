using AutoMapper;
using EmployeeDayOffManagement.Application.Interfaces;
using EmployeeDayOffManagement.Core.Dtos.LeaveAllocation;
using EmployeeDayOffManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDayOffManagement.Application.LeaveAllocation;

public class LeaveAllocationService : ILeaveAllocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LeaveAllocationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<LeaveAllocationDto>> GetAllAllocations()
        {
            var allocations = await _unitOfWork.Repository<Core.Entities.LeaveAllocation>()
                .GetAll()
                .ToListAsync();

            return _mapper.Map<List<LeaveAllocationDto>>(allocations);
        }

        public async Task<LeaveAllocationDto> GetAllocationById(int id)
        {
            var allocation = await _unitOfWork.Repository<Core.Entities.LeaveAllocation>()
                .GetById(x => x.Id == id).FirstOrDefaultAsync();

            if (allocation == null)
            {
                return null;
            }

            return _mapper.Map<LeaveAllocationDto>(allocation);
        }

        public async Task CreateAllocation(CreateLeaveAllocationDto createDto)
        {
            var allocation = _mapper.Map<Core.Entities.LeaveAllocation>(createDto);
            _unitOfWork.Repository<Core.Entities.LeaveAllocation>().Create(allocation);
            _unitOfWork.Complete();
        }

        public async Task UpdateAllocation(LeaveAllocationDto allocationdto)
        {
            var allocation = await GetAllocationById(allocationdto.Id);

            if (allocation != null)
            {
                allocation.EmployeeId = allocationdto.EmployeeId;
                allocation.LeaveTypeId = allocationdto.LeaveTypeId;
                allocation.NumberOfDays = allocationdto.NumberOfDays;
                allocation.Period = allocationdto.Period;
                allocation.DateCreated = allocationdto.DateCreated;
            }

            var allocationUpdated = _mapper.Map<Core.Entities.LeaveAllocation>(allocationdto);
            _unitOfWork.Repository<Core.Entities.LeaveAllocation>().Update(allocationUpdated);
            _unitOfWork.Complete();
        }

        public async Task DeleteAllocation(int id)
        {
            var allocation = await _unitOfWork.Repository<Core.Entities.LeaveAllocation>()
                .GetById(x => x.Id == id).FirstOrDefaultAsync();

            _unitOfWork.Repository<Core.Entities.LeaveAllocation>().Delete(allocation);
            _unitOfWork.Complete();
        }

        public async Task AccumulateAnnualLeave()
        {
            var currentYear = DateTime.Now.Year;
            var employees = await _unitOfWork.Repository<Core.Entities.Employee>()
                .GetAll()
                .ToListAsync();

            var leaveTypes = await _unitOfWork.Repository<LeaveType>()
                .GetAll()
                .Where(l => l.Name == "Annual")
                .ToListAsync();

            foreach (var employee in employees)
            {
                var leaveAllocation = await _unitOfWork.Repository<Core.Entities.LeaveAllocation>()
                    .GetAll()
                    .FirstOrDefaultAsync(
                        l => l.EmployeeId == employee.Id 
                        && l.LeaveTypeId == leaveTypes.First().Id
                        && l.Period == currentYear
                    );

                if (leaveAllocation == null)
                {
                    leaveAllocation = new Core.Entities.LeaveAllocation
                    {
                        EmployeeId = employee.Id,
                        LeaveTypeId = leaveTypes.First().Id,
                        NumberOfDays = 0,
                        Period = currentYear,
                        DateCreated = DateTime.Now
                    };
                    _unitOfWork.Repository<Core.Entities.LeaveAllocation>().Create(leaveAllocation);
                }

                // Accumulate 2 days per month
                leaveAllocation.NumberOfDays += 2;
                _unitOfWork.Repository<Core.Entities.LeaveAllocation>().Update(leaveAllocation);
            }

            _unitOfWork.Complete();
        }

        public async Task ResetLeaveAtYearStart()
        {
            var previousYear = DateTime.Now.Year - 1;

            var leaveAllocations = await _unitOfWork.Repository<Core.Entities.LeaveAllocation>()
                .GetAll()
                .Where(l => l.Period == previousYear)
                .ToListAsync();

            foreach (var allocation in leaveAllocations)
            {
                var currentYearAllocation = await _unitOfWork.Repository<Core.Entities.LeaveAllocation>()
                    .GetAll()
                    .FirstOrDefaultAsync(
                        l => l.EmployeeId == allocation.EmployeeId 
                        && l.LeaveTypeId == allocation.LeaveTypeId
                        && l.Period == DateTime.Now.Year
                    );

                if (currentYearAllocation == null)
                {
                    currentYearAllocation = new Core.Entities.LeaveAllocation
                    {
                        EmployeeId = allocation.EmployeeId,
                        LeaveTypeId = allocation.LeaveTypeId,
                        NumberOfDays = 0,
                        Period = DateTime.Now.Year,
                        DateCreated = DateTime.Now
                    };
                    _unitOfWork.Repository<Core.Entities.LeaveAllocation>().Create(currentYearAllocation);
                }
                else
                {
                    // Carry over days from previous year
                    currentYearAllocation.NumberOfDays = allocation.NumberOfDays;
                    _unitOfWork.Repository<Core.Entities.LeaveAllocation>().Update(currentYearAllocation);
                }

                // Delete the old allocation
                _unitOfWork.Repository<Core.Entities.LeaveAllocation>().Delete(allocation);
            }

            _unitOfWork.Complete();
        }
    }