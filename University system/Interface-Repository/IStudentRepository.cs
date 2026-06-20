using University_system.Models;

namespace University_system.Interfaces
{
    public interface IStudentRepository :IGenericRepository<Student>

    {
        Task<Student> GetStudentByNumberWithDetailsAsync(string studentnumber);
    }
}
