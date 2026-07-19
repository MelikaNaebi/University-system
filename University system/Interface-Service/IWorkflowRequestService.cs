using University_system.Dtos;

namespace University_system.Interface_Service
{
    public interface IWorkflowRequestService
    {
        Task<bool> CreateRequestAsync(int studentId, int templateId, string title, string description);

        Task<IEnumerable<WorkflowRequestDto>> GetStudentRequestsAsync(int studentId);

        Task<bool> ReviewByStaffAsync(ReviewWorkflowRequestDto dto);

        Task<bool> ReviewByManagerAsync(ReviewWorkflowRequestDto dto);

        Task<IEnumerable<WorkflowRequestDto>> GetStaffCartableAsync();

        Task<IEnumerable<WorkflowRequestDto>> GetManagerCartableAsync(int semesterId);


        Task<bool> CreateTemplateAsync(CreateTemplateDto dto);

        Task<IEnumerable<WorkflowTemplateDto>> GetAllTemplatesAsync();
    }
}