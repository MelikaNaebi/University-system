using Microsoft.EntityFrameworkCore;
using University_system.Data;
using University_system.Interface_Repository;
using University_system.Models;

namespace University_system.Repositories
{
    public class WorkflowRequestRepository:GenericRepository<WorkflowRequest>,IWorkflowRequestRepository
    {
        public WorkflowRequestRepository(DataContext context) : base(context) { }


    public async Task<IEnumerable<WorkflowRequest>> GetWorkFlowRequestsByStudentIdAsync(int studentId, int semesterId)
        {
            return await _dbSet
    .Include(w => w.WorkflowTemplate)
    .Include(w => w.Student)             
        .ThenInclude(s => s.User)       
    .Where(w => w.StudentId == studentId && w.SemesterId == semesterId)
    .OrderByDescending(w => w.CreatedAt)
    .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowRequest>> GetPendingRequestsForStaffAsync(int semesterId)
        {
            return await _dbSet
                .Include(w => w.WorkflowTemplate) 
                .Include(w => w.Student)
                    .ThenInclude(s => s.User)    
                .Where(w => w.SemesterId == semesterId && w.Status == "Pending")
                .OrderBy(w => w.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowRequest>> GetPendingRequestsForManagerAsync(int semesterId)
        {
            return await _dbSet
                .Include(w => w.WorkflowTemplate) 
                .Include(w => w.Student)
                    .ThenInclude(s => s.User)   
                .Where(w => w.SemesterId == semesterId && w.Status == "ApprovedByStaff")
                .OrderBy(w => w.CreatedAt)
                .ToListAsync();
        }
        public async Task<WorkflowRequest> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(w => w.WorkflowTemplate).FirstOrDefaultAsync(w => w.Id == id);

        }
    }
}
