using University_system.Data;
using University_system.Interfaces;
using University_system.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace University_system.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Course>> GetCoursesByInstructorIdAndSemesterIdAsync( int instructorId)
        {
            var activeSemester = await _context.Semesters
        .Where(s => s.IsCurrent)
        .Select(s => s.Id)
        .FirstOrDefaultAsync();

            return await _dbSet.Where(c => c.SemesterId == activeSemester && c.InstructorId == instructorId).ToListAsync();


        }

        public async Task<IEnumerable<Course>> GetCoursesByInstrctorIdAndSemasterIdAsync(int semasterId)
        {
            
            return await _dbSet.Where(c => c.SemesterId == semasterId).Include(c => c.Instructor)
                .ThenInclude(i => i.User).Include(c => c.Semester).ToListAsync();

        }

        public async Task<IEnumerable<Course>> GetCoursesByInstructorIdAndSemesterIdAsync(int semasterId, int instructorId)
        {
            return await _dbSet.Where(c => c.SemesterId == semasterId && c.InstructorId==instructorId).Include(c => c.Instructor).ToListAsync();
        }
    }
}