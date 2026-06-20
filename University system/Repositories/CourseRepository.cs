using University_system.Data;
using University_system.Interfaces;
using University_system.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace University_system.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(DataContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Course>> GetCoursesByInstrctorIdAndSemasterIdAsync( int semesterId)
        {
            return await _dbSet.Include(c=>c.Instructor).ThenInclude(i => i.User).Include(c => c.Semaster)
                .Where(c => c.SemasterId == semesterId)
                .ToListAsync();
        }
    }
}
