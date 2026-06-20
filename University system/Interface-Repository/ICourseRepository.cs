using University_system.Models;

namespace University_system.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetCoursesByInstrctorIdAndSemasterIdAsync( int semesterId);
    }
}
