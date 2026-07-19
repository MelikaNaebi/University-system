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

        public WorkflowRequestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateRequestAsync(int studentId,  int templateId, string title, string description)
        {
            var template = await _unitOfWork.WorkflowTemplates.GetByIdAsync(templateId);
            if (template == null) return false;

            string initialStatus = template.RequiresStaffApproval ? "Pending" : "ApprovedByStaff";
            var activeSemesterId = await _unitOfWork.Semesters.GetCurrentSemesterAsync();


            var request = new WorkflowRequest
            {
                StudentId = studentId,
                WorkflowTemplateId = templateId,
                Title = title,
                Description = description,
                SemesterId = activeSemesterId,
                Status = initialStatus,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.WorkflowRequests.AddAsync(request);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }
        public async Task<IEnumerable<WorkflowRequestDto>> GetStudentRequestsAsync(int studentId)
        {
            var activeSemesterId = await _unitOfWork.Semesters.GetCurrentSemesterAsync();

            var studentRequests = await _unitOfWork.WorkflowRequests.GetWorkFlowRequestsByStudentIdAsync(studentId, activeSemesterId);
            if (studentRequests == null) { return null; }

            var dtos = _mapper.Map<IEnumerable<WorkflowRequestDto>>(studentRequests);

            foreach (var dto in dtos)
            {
                var originalRequest = studentRequests.FirstOrDefault(r => r.Id == dto.Id);
                if (originalRequest?.WorkflowTemplate != null)
                {
                    dto.TemplateName = originalRequest.WorkflowTemplate.Name;
                }
            }

            return dtos;
        }

        public async Task<bool> ReviewByStaffAsync(ReviewWorkflowRequestDto dto)
        {
            var request = await _unitOfWork.WorkflowRequests.GetByIdAsync(dto.RequestId);
            if (request == null || request.Status != "Pending") return false;

            request.StaffComment = dto.Comment;
            request.Status = dto.IsApproved ? "ApprovedByStaff" : "RejectedByStaff";

            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<bool> ReviewByManagerAsync(ReviewWorkflowRequestDto dto)
        {
            var request = await _unitOfWork.WorkflowRequests.GetByIdAsync(dto.RequestId);

            if (request == null || request.Status != "ApprovedByStaff") return false;

            request.ManagerComment = dto.Comment;
            request.Status = dto.IsApproved ? "ApprovedByManager" : "RejectedByManager";

            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<IEnumerable<WorkflowRequestDto>> GetStaffCartableAsync()
        {
            var activeSemesterId = await _unitOfWork.Semesters.GetCurrentSemesterAsync();

            var requests = await _unitOfWork.WorkflowRequests.GetPendingRequestsForStaffAsync(activeSemesterId);

            var dtos = _mapper.Map<IEnumerable<WorkflowRequestDto>>(requests).ToList();

            for (int i = 0; i < requests.Count(); i++)
            {
                var req = requests.ElementAt(i);
                var dto = dtos.ElementAt(i);

                dto.TemplateName = req.WorkflowTemplate != null ? req.WorkflowTemplate.Name : "قالب نامشخص";

                if (req.Student?.User != null)
                {
                    dto.StudentName = $"{req.Student.User.FirstName} {req.Student.User.LastName}";
                }
            }

            return dtos;
        }

        public async Task<IEnumerable<WorkflowRequestDto>> GetManagerCartableAsync(int semesterId)
        {
            var requests = await _unitOfWork.WorkflowRequests.GetPendingRequestsForManagerAsync(semesterId);

            var dtos = _mapper.Map<IEnumerable<WorkflowRequestDto>>(requests).ToList();

            for (int i = 0; i < requests.Count(); i++)
            {
                var req = requests.ElementAt(i);
                var dto = dtos.ElementAt(i);

                dto.TemplateName = req.WorkflowTemplate != null ? req.WorkflowTemplate.Name : "قالب نامشخص";

                if (req.Student?.User != null)
                {
                    dto.StudentName = $"{req.Student.User.FirstName} {req.Student.User.LastName}";
                }
            }

            return dtos;
        }

        public async Task<bool> CreateTemplateAsync(CreateTemplateDto dto)
        {
            var template = new WorkflowTemplate
            {
                Name = dto.Name,
                Description = dto.Description,
                RequiresStaffApproval = dto.RequiresStaffApproval,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.WorkflowTemplates.AddAsync(template);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<IEnumerable<WorkflowTemplateDto>> GetAllTemplatesAsync()
        {
            var templates = await _unitOfWork.WorkflowTemplates.GetAllAsync();
            return _mapper.Map<IEnumerable<WorkflowTemplateDto>>(templates);
        }
    }
}