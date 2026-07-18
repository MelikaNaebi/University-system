using University_system.Interfaces;
using University_system.Models;

namespace University_system.Interface_Repository
{
    public interface IUnitOfWork
    {
        ICourseRepository Courses { get; }
        IEnrollmentRepository Enrollments { get; } 
        IStudentRepository Students { get; }
        IGenericRepository<WorkflowTemplate> WorkflowTemplates { get; }
        IWorkflowRequestRepository WorkflowRequests { get; }
        ISemesterRepository Semesters { get; }

        IInstructorRepository Instructors { get; }


        Task<int> CompleteAsync();
    }
}
