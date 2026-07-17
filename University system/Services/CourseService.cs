using AutoMapper;
using University_system.Dtos;
using University_system.Interface_Repository;
using University_system.Interface_Service;
using University_system.Interfaces;
using University_system.Models;

namespace University_system.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddNewCourseByAdmin(string title, string unit, int capacity, int semesterId, int instructorId)
        {
            var instructorCourses = await _unitOfWork.Courses.GetCoursesByInstructorIdAndSemesterIdAsync(semesterId, instructorId);

            bool isDuplicate = instructorCourses.Any(c => c.Title == title);
            if (isDuplicate) {

                return false;
            }
         
            var course = new Course {
                Title = title,
                Unit = unit,
                Capacity = capacity,
                SemesterId = semesterId,
                EnrolledCount = 0,
                InstructorId = instructorId

            };
            await _unitOfWork.Courses.AddAsync(course);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0;

        }

        public async Task<IEnumerable<AvailableCourseDto>> GetAvailableCoursesAsync()
        {
            var activeSemesterId = await _unitOfWork.Semesters.GetCurrentSemesterAsync();
            var courses= await _unitOfWork.Courses.GetCoursesByInstrctorIdAndSemasterIdAsync(activeSemesterId);

            var courseDtos= _mapper.Map<IEnumerable<AvailableCourseDto>>(courses);

            return courseDtos;
        }

        public async Task<IEnumerable<CourseDto>> GetInstructorCoursesAsync( int instructorId)
        {

            var instructorcourses = await _unitOfWork.Courses.GetCoursesByInstructorIdAndSemesterIdAsync( instructorId);
            var instructorcoursesdto= _mapper.Map<IEnumerable<CourseDto>>(instructorcourses);
            return instructorcoursesdto;

        }
    }
}
