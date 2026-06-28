using University_system.Interfaces;
using University_system.Models;

namespace University_system.Interface_Repository
{
    public interface IWorkflowRequestRepository : IGenericRepository<WorkflowRequest>
    {
        Task<IEnumerable<WorkflowRequest>> GetWorkFlowRequestsByStudentIdAsync(int studentId , int semesterId);
    }
}
