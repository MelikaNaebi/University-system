using University_system.Interfaces;
using University_system.Models;

namespace University_system.Interface_Repository
{
    public interface IWorkflowRequestRepository : IGenericRepository<WorkflowRequest>
    {
        Task<IEnumerable<WorkflowRequest>> GetWorkFlowRequestsByStudentIdAsync(int studentId , int semesterId);
        Task<IEnumerable<WorkflowRequest>> GetPendingRequestsForStaffAsync(int semesterId);

        Task<IEnumerable<WorkflowRequest>> GetPendingRequestsForManagerAsync(int semesterId);

        Task<WorkflowRequest> GetByIdAsync(int id);
    }
}
