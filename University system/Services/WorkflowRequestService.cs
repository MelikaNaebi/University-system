using AutoMapper;
using University_system.Dtos;
using University_system.Interface_Repository;
using University_system.Interface_Service;
using University_system.Models;

namespace University_system.Services
{
    public class WorkflowRequestService : IWorkflowRequestService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkflowRequestService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<bool> CreateRequestAsync(int studentId, int semesterId, string title, string description)
        {
            var request = new WorkflowRequest { 
            
                StudentId = studentId,
                Title = title,
                Description = description,
                SemesterId = semesterId,

                Status = "در حال بررسی",
                CreatedAt = DateTime.Now

            };

            await _unitOfWork.WorkflowRequests.AddAsync(request);

            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<IEnumerable<WorkflowRequestDto>> GetStudentRequestsAsync(int studentId, int semesterId)
        {
            var studentRequests = await _unitOfWork.WorkflowRequests.GetWorkFlowRequestsByStudentIdAsync(studentId, semesterId);
            if (studentRequests == null) { return null; }

            return _mapper.Map<IEnumerable<WorkflowRequestDto>>(studentRequests);

        }
    }
}
