using University_system.Dtos;

namespace University_system.Interface_Service
{
    public interface ICourseService
    {
        Task<IEnumerable<AvailableCourseDto>> GetAvailableCoursesAsync(int semasterId);
    }
}
