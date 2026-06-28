using University_system.Dtos;

namespace University_system.Interface_Service
{
    public interface IStudentService
    {
        Task<StudentProfileDto> GetStudentProfileAsync(int studentId);


    }
}
