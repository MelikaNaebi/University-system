using University_system.Dtos;
using University_system.Models;

namespace University_system.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetCoursesByInstructorIdAndSemesterIdAsync( int instructorId);

        Task<IEnumerable<Course>> GetCoursesByInstrctorIdAndSemasterIdAsync(int semasterId);

        Task<IEnumerable<Course>> GetCoursesByInstructorIdAndSemesterIdAsync(int semasterId, int instructorId);



    }
}
