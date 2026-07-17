using AutoMapper;
using University_system.Dtos;
using University_system.Interface_Repository;
using University_system.Interface_Service;
using University_system.Interfaces;
using University_system.Models;

namespace University_system.Services
{
    public class StudentService : IStudentService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FullTranscriptDto> GetFullTranscriptAsync(int studentId)
        {
            var Enrolledcourses= await _unitOfWork.Enrollments.GetAllStudentEnrollmentsAsync(studentId);

            var coursesGroupedBySemester = Enrolledcourses.GroupBy(e => e.Course.SemesterId);

            var semesterTranscripts = new List<SemesterTranscriptDto>();

            foreach (var semester in coursesGroupedBySemester) {

                var semesterDto = CalculateSemesterTranscript(semester.Key, semester);

                semesterTranscripts.Add(semesterDto);
            }

            int totalUnits = Enrolledcourses.Sum(e => int.Parse(e.Course.Unit));

            double totalPoints = Enrolledcourses.Sum(e =>(e.Grade ??0 )* int.Parse(e.Course.Unit));

            double totalGpa = totalUnits > 0 ? totalPoints / totalUnits : 0;
            return new FullTranscriptDto
            {
                StudentId = studentId,
                TotalGpa = totalGpa,
                TotalCredits = totalUnits,
                Semesters = semesterTranscripts
            };
        }

        public async Task<SemesterTranscriptDto> GetSemesterTranscriptAsync(int studentId, int semesterId)
        {
            var semesterCourses = await _unitOfWork.Enrollments.GetStudentCoursesBySemesterAsync(studentId, semesterId);


            var transcriptDto = CalculateSemesterTranscript(semesterId, semesterCourses);

            transcriptDto.Courses = semesterCourses.Select(e => new EnrolledCourseDto
            {
                 Id = e.Id, // شناسه خود رکورد ثبت نام (EnrollmentId)
                 CourseId = e.CourseId,
                 Grade = e.Grade,
                  CourseTitle = e.Course?.Title ?? "نامشخص", // نام فیلد در دیتابیس (Title یا Name)
                    CourseUnit = e.Course?.Unit ?? "0",        // تعداد واحد درس به صورت رشته
                  InstructorName = e.Course?.Instructor?.User != null
                 ? $"{e.Course.Instructor.User.FirstName} {e.Course.Instructor.User.LastName}"
    : "نامشخص"
            }).ToList();

            return transcriptDto;
        }

        public async Task<StudentProfileDto> GetStudentProfileAsync(int studentId)
        {
            var studentProfile = await _unitOfWork.Students.GetStudentWithUserAsync(studentId);

            if (studentProfile == null) return null;

            return _mapper.Map<StudentProfileDto>(studentProfile);

        }
        private SemesterTranscriptDto CalculateSemesterTranscript(int semesterId, IEnumerable<Enrollment> semesterEnrollments)
        {

            int totalCredits = semesterEnrollments.Sum(e => int.Parse(e.Course.Unit));

            double totalPoints = semesterEnrollments.Sum(e => (e.Grade ?? 0)* int.Parse(e.Course.Unit));

            double gpa = totalCredits > 0 ? totalPoints / totalCredits : 0;
            return new SemesterTranscriptDto
            {
                SemesterId = semesterId,
                SemesterGpa = gpa,
                SemesterCredits = totalCredits
            };

        }
    }
}
