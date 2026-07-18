using University_system.Dtos;

namespace University_system.Interface_Service
{
    public interface IStudentService
    {
        Task<StudentProfileDto> GetStudentProfileAsync(int studentId);

        Task<SemesterTranscriptDto> GetSemesterTranscriptAsync(int studentId, int semesterId);

        Task<FullTranscriptDto> GetFullTranscriptAsync(int studentId);

        Task<int> GetCurrentSemesterIdAsync();

        Task<List<StudentReportResponseDto>> GetStudentsReportAsync(StudentReportFilterDto filter);
    }
}
