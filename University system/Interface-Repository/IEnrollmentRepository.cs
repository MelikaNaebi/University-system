using University_system.Interfaces;
using University_system.Models;

namespace University_system.Interface_Repository
{
    public interface IEnrollmentRepository: IGenericRepository<Enrollment>
    {
        Task<IEnumerable<Enrollment>> GetStudentCoursesBySemesterAsync(int studentId, int semesterId);

        Task<IEnumerable<Enrollment>> GetAllStudentEnrollmentsAsync(int studentId);
    }
}
