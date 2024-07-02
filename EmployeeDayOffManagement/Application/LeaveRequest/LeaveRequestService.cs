using AutoMapper;
using EmployeeDayOffManagement.Application.Interfaces;
using EmployeeDayOffManagement.Core.Dtos.LeaveRequest;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDayOffManagement.Application.LeaveRequest;

public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LeaveRequestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<LeaveRequestDto>> GetAllRequests(string employeeId)
        {
            var requests = await _unitOfWork.Repository<Core.Entities.LeaveRequest>().GetByCondition(r => r.RequestingEmployeeId == employeeId).ToListAsync();
            return _mapper.Map<List<LeaveRequestDto>>(requests);
        }

        public async Task<LeaveRequestDto> GetRequest(int id)
        {
            var request = await _unitOfWork.Repository<Core.Entities.LeaveRequest>().GetByCondition(r => r.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<LeaveRequestDto>(request);
        }

        public async Task CreateRequest(CreateLeaveRequestDto requestDto)
        {
            var request = _mapper.Map<Core.Entities.LeaveRequest>(requestDto);
            _unitOfWork.Repository<Core.Entities.LeaveRequest>().Create(request);
            _unitOfWork.Complete();
        }

        public async Task UpdateRequest(LeaveRequestDto requestDto)
        {
            var request = await GetRequest(requestDto.Id);

            if (request != null)
            {
                request.EmployeeId = requestDto.EmployeeId;
                request.LeaveTypeId = requestDto.LeaveTypeId;
                request.StartDate = requestDto.StartDate;
                request.EndDate = requestDto.EndDate;
                request.LeadId = requestDto.LeadId;
                request.IsApproved = requestDto.IsApproved;
            }

            var requestUpdated = _mapper.Map<Core.Entities.LeaveRequest>(requestDto);
            _unitOfWork.Repository<Core.Entities.LeaveRequest>().Update(requestUpdated);
            _unitOfWork.Complete();
        }

        public async Task DeleteRequest(int id)
        {
            var request = await _unitOfWork.Repository<Core.Entities.LeaveRequest>().GetByCondition(r => r.Id == id).FirstOrDefaultAsync();
            if (request != null)
            {
                _unitOfWork.Repository<Core.Entities.LeaveRequest>().Delete(request);
                _unitOfWork.Complete();
            }
        }
    }