using Microsoft.EntityFrameworkCore;
using University_system.Data;
using University_system.Interface_Repository;
using University_system.Models;

namespace University_system.Repositories
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        
        public EnrollmentRepository(DataContext context) : base(context) { }
        
        public async Task<IEnumerable<Enrollment>> GetAllStudentEnrollmentsAsync(int studentId)
        {
            return await _dbSet.Include(e => e.Course).Where(e => e.StudentId == studentId).ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetStudentCoursesBySemesterAsync(int studentId, int semesterId)
        {
            return await _dbSet
        .Include(e => e.Course)
        .ThenInclude(c => c.Instructor)
        .ThenInclude(i => i.User)
        .Where(e => e.StudentId == studentId && e.Course.SemasterId == semesterId)
        .ToListAsync();
        }
    }
}
