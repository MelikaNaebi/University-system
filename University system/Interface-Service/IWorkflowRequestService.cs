using University_system.Dtos;

namespace University_system.Interface_Service
{
    public interface IWorkflowRequestService
    {
        Task<bool> CreateRequestAsync(int studentId, int semesterId, string title, string description);

        Task<IEnumerable<WorkflowRequestDto>> GetStudentRequestsAsync(int studentId,int semesterId);
    }
}
