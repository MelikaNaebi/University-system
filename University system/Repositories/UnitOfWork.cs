using University_system.Data;
using University_system.Interface_Repository;
using University_system.Interfaces;
using University_system.Models;

namespace University_system.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public ICourseRepository Courses { get; private set; }
        public IEnrollmentRepository Enrollments { get; private set; }
        public IStudentRepository Students { get; private set; }
        public IInstructorRepository Instructors { get; private set; }
        public ISemesterRepository Semesters { get; private set; }
        public IWorkflowRequestRepository WorkflowRequests { get; private set; }

        public IGenericRepository<WorkflowTemplate> WorkflowTemplates { get; private set; }

        public UnitOfWork(DataContext context)
        {
            _context = context;
            Instructors = new InstructorRepository(_context);
            Courses = new CourseRepository(_context);
            Enrollments = new EnrollmentRepository(_context);
            Students = new StudentRepository(_context);
            WorkflowRequests = new WorkflowRequestRepository(_context);
            Semesters = new SemesterRepository(_context);

            WorkflowTemplates = new GenericRepository<WorkflowTemplate>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}