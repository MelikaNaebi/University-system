using AutoMapper;
using System.Diagnostics;
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

            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);

            _unitOfWork.Enrollments.Delete(enrollment);

            if (course != null && course.EnrolledCount > 0)
            {
                course.EnrolledCount--;
                _unitOfWork.Courses.Update(course);
            }

            var result = await _unitOfWork.CompleteAsync();
            return result > 0;

        }

        public async Task<bool> EnrollCourseAsync(int courseId, int studentId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
            {
                throw new KeyNotFoundException("درس مورد نظر در سیستم یافت نشد.");
            }

            if(course.EnrolledCount >=course.Capacity) {

                throw new InvalidOperationException("ظرفیت این درس تکمیل شده است.");
            }

            var isAlreadyEnrolled = await _unitOfWork.Enrollments.IsStudentEnrolledAsync(studentId, courseId);

            if (isAlreadyEnrolled) {
                throw new InvalidOperationException("شما قبلاً این درس را اخذ کرده‌اید.");
            }

            var enrollment = new Enrollment { StudentId = studentId, CourseId = courseId };
            await _unitOfWork.Enrollments.AddAsync(enrollment);

            course.EnrolledCount++;
            _unitOfWork.Courses.Update(course);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
            {
                throw new Exception("خطای غیرمنتظره‌ای در ثبت اطلاعات در دیتابیس رخ داد.");
            }
            return true;
        }

        
        public async Task<IEnumerable<StudentForGradeDto>> GetCourseStudentsbycourseAsync(int coursrId)
        {
            var enrollments = await _unitOfWork.Enrollments.GetStudentCoursesByCourseIdAsync(coursrId);

            var studentsDto = _mapper.Map<IEnumerable<StudentForGradeDto>>(enrollments);

            return studentsDto;
        }



        public async Task<IEnumerable<EnrolledCourseDto>> GetStudentCoursesForActiveSemesterAsync(int studentId)
        {
            var enrollments = await _unitOfWork.Enrollments.GetStudentCoursesForActiveSemesterAsync(studentId);

            if (enrollments == null)
            {
                throw new Exception("دروسی برای این دانشجو در ترم فعال یافت نشد.");
            }

            var enrollmentsDto = _mapper.Map<IEnumerable<EnrolledCourseDto>>(enrollments);

            foreach (var dto in enrollmentsDto)
            {
                var original = enrollments.FirstOrDefault(e => e.Id == dto.Id);

                if (original?.Course?.Instructor != null)
                {
                    var user = original.Course.Instructor.User;

                    if (user != null && (!string.IsNullOrEmpty(user.FirstName) || !string.IsNullOrEmpty(user.LastName)))
                    {
                        dto.InstructorName = $"{user.FirstName} {user.LastName}";
                    }
                    else
                    {
                        dto.InstructorName = "استاد بدون نام";
                    }
                }
                else
                {
                    dto.InstructorName = "بدون استاد";
                }
            }

            return enrollmentsDto;
        }

        public async Task<bool> SubmitOrUpdateGradeByInstructorAsync(int studentId, int courseId, double grade)
        {
            var enrolledcourse = await _unitOfWork.Enrollments.GetEnrollmentAsync(studentId,courseId);

            if (enrolledcourse == null)
            {
                return false;
            }

            enrolledcourse.Grade = grade;
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

       
    }
}
