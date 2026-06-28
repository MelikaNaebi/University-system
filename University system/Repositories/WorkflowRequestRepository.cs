using Microsoft.EntityFrameworkCore;
using University_system.Data;
using University_system.Interface_Repository;
using University_system.Models;

namespace University_system.Repositories
{
    public class WorkflowRequestRepository:GenericRepository<WorkflowRequest>,IWorkflowRequestRepository
    {
        public WorkflowRequestRepository(DataContext context):base(context) { 
        
        }

        public async Task<IEnumerable<WorkflowRequest>> GetWorkFlowRequestsByStudentIdAsync(int studentId,int semesterId)
        {
            return await _dbSet.Where(w =>w.Student.Id==studentId && w.SemesterId==semesterId).ToListAsync();
        }
    }
}
