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

        public async Task<int> GetCurrentSemesterIdAsync()
        {
            var currentSemesterId = await _unitOfWork.Semesters.GetCurrentSemesterAsync();
            if (currentSemesterId == 0)
            {
                throw new InvalidOperationException("هیچ ترمی در سیستم به عنوان ترم جاری (IsCurrent) فعال نشده است.");
            }

            return currentSemesterId;
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
                 Id = e.Id, 
                 CourseId = e.CourseId,
                 Grade = e.Grade,
                  CourseTitle = e.Course?.Title ?? "نامشخص", 
                    CourseUnit = e.Course?.Unit ?? "0",       
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

        public async Task<List<StudentReportResponseDto>> GetStudentsReportAsync(StudentReportFilterDto filter)
        {
            var students = await _unitOfWork.Students.GetFilteredStudentsAsync(filter.StudentNumber, filter.Name);

            var resultList = new List<StudentReportResponseDto>();
            foreach (var student in students)
            {
                var semesterEnrollments = student.Enrollments
                    .Where(e => e.Course.SemesterId == filter.SemesterId && e.Grade != null)
                    .ToList();

                if (!semesterEnrollments.Any()) continue;

                double totalWeightedGrades = 0;
                int totalUnits = 0;

                foreach (var enrollment in semesterEnrollments)
                {
                    if (int.TryParse(enrollment.Course.Unit, out int courseUnit))
                    {
                        totalWeightedGrades += enrollment.Grade.Value * courseUnit;
                        
                        totalUnits += courseUnit;
                    }
                }

                double semesterGpa = totalUnits > 0 ? totalWeightedGrades / totalUnits : 0;
                semesterGpa = Math.Round(semesterGpa, 2);

                string status = "عادی";
                if (semesterGpa < 12) status = "مشروط";
                else if (semesterGpa >= 17) status = "ممتاز";

                if (filter.StatusFilter.ToLower() == "probation" && status != "مشروط") continue;
                if (filter.StatusFilter.ToLower() == "top" && status != "ممتاز") continue;

                resultList.Add(new StudentReportResponseDto
                {
                    Id = student.Id,
                    StudentNumber = student.StudentNumber,
                    FullName = student.User.FirstName + " " + student.User.LastName,
                    GPA = semesterGpa,
                    Status = status
                });
            }

            return resultList;
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
