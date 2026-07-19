using Microsoft.EntityFrameworkCore;
using University_system.Data;
using University_system.Dtos;
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
        .Where(e => e.StudentId == studentId && e.Course.SemesterId == semesterId)
        .ToListAsync();
        }

        public async Task<Enrollment> GetEnrollmentAsync(int studenId, int courseId)
        {
            return await _dbSet.FirstOrDefaultAsync(e =>e.StudentId==studenId && e.CourseId==courseId);
        }

        public async Task<bool> IsStudentEnrolledAsync(int studentId, int courseId)
        {
            return  await _dbSet.AnyAsync(e=>e.StudentId==studentId && e.CourseId==courseId);
        }

        public async Task<IEnumerable<Enrollment>> GetStudentCoursesForActiveSemesterAsync(int studentId)
        {

            var activeSemester = await _context.Semesters
             .Where(s => s.IsCurrent)
             .Select(s => s.Id)
                .FirstOrDefaultAsync();


            return await _dbSet.Include(e => e.Course).ThenInclude(c => c.Instructor).ThenInclude(i => i.User).Where(e => e.StudentId == studentId && e.Course.SemesterId == activeSemester).ToListAsync(); ;
        }

        public async Task<IEnumerable<Enrollment>> GetStudentCoursesByCourseIdAsync(int coursrId)
        {
            return await _dbSet.Where(e=>e.CourseId == coursrId).Include(e =>e.Student ).ThenInclude(s =>s.User).ToListAsync();

        }
    }
}
