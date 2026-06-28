using AutoMapper;
using University_system.Dtos;
using University_system.Interface_Repository;
using University_system.Interface_Service;
using University_system.Models;

namespace University_system.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> DropCourseAsync(int courseId, int studentId)
        {
            var enrollment = await _unitOfWork.Enrollments.GetEnrollmentAsync(studentId, courseId);

            if (enrollment == null) return false;

            var course = await _unitOfWork.Courses.GetById(courseId);

            _unitOfWork.Enrollments.Delete(enrollment);

            if (course != null && course.EnrolledCount > 0)
            {
                course.EnrolledCount--;
                _unitOfWork.Courses.Update(course);
            }

            var result = await _unitOfWork.CompleteAsync();
            return result > 0;

        }

        public async Task<bool> EnrollCourse(int courseId, int studentId)
        {
            var course= await _unitOfWork.Courses.GetById(courseId);

            if(course.EnrolledCount >=course.Capacity) { return false; }

            var isAlreadyEnrolled = await _unitOfWork.Enrollments.IsStudentEnrolledAsync(studentId, courseId);

            if (isAlreadyEnrolled) return false;

            var enrollment = new Enrollment { StudentId = studentId, CourseId = courseId };
            await _unitOfWork.Enrollments.AddAsync(enrollment);

            course.EnrolledCount++;
            _unitOfWork.Courses.Update(course);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0;

        }

        public async Task<IEnumerable<EnrolledCourseDto>> GetEnrolledCourses(int studentId)
        {
            var enrollments = await _unitOfWork.Enrollments.GetAllStudentEnrollmentsAsync(studentId);

            var courses = enrollments.Select(e => e.Course).ToList();

            var courseDtos = _mapper.Map<IEnumerable<EnrolledCourseDto>>(courses);
            return courseDtos;
        }

      
    }
}
