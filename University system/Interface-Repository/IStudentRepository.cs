using University_system.Models;

namespace University_system.Interfaces
{
    public interface IStudentRepository :IGenericRepository<Student>

    {
        Task<Student> GetStudentWithUserAsync(int studentId );
        Task<int> GetStudentIdByUserIdAsync(string userId);

        Task<List<Student>> GetFilteredStudentsAsync(string? studentNumber, string? name);

    }
}
