using University_system.Dtos;
using University_system.Models;

namespace University_system.Interface_Service
{
    public interface IEnrollmentService
    {
       Task<bool> EnrollCourse(int courseId, int studentId);

        Task<IEnumerable<EnrolledCourseDto>> GetEnrolledCourses( int studentId);

        Task<bool> DropCourseAsync(int courseId, int studentId);
    }
}
