using University_system.Dtos;
using University_system.Models;

namespace University_system.Interface_Service
{
    public interface ICourseService
    {
        Task<IEnumerable<AvailableCourseDto>> GetAvailableCoursesAsync();

        Task<bool> AddNewCourseByAdmin(string title, string unit, int capacity, int semesterId, int instructorId);

        Task<IEnumerable<CourseDto>> GetInstructorCoursesAsync( int instructorId);
    }
}
