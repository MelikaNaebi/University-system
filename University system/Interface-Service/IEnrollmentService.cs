using University_system.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace University_system.Interface_Service
{
    public interface IEnrollmentService
    {
        Task<bool> EnrollCourseAsync(int courseId, int studentId);
        Task<bool> DropCourseAsync(int courseId, int studentId);

        // اضافی ؟ فقط ریپو کافی نیست برای این ؟
        Task<IEnumerable<EnrolledCourseDto>> GetStudentCoursesBySemesterAsync(int studentId, int semesterId);
        // اضافی ؟ فقط ریپو کافی نیست برای این ؟

        Task<IEnumerable<EnrolledCourseDto>> GetAllEnrolledCoursesAsync(int studentId);
        Task<IEnumerable<EnrolledCourseDto>> GetStudentCoursesForActiveSemesterAsync(int studentId);

        Task<bool> SubmitGradeByInstructorAsync(int enrollmentId, double grade);
        Task<bool> UpdateGradeAsync(int enrollmentId, double newGrade);
    }
}