using AutoMapper;
using University_system.Dtos;
using University_system.Models;

namespace University_system.Helper
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<Course, AvailableCourseDto>()
                .ForMember(dest => dest.InstructorName,
                           opt => opt.MapFrom(src => src.Instructor != null && src.Instructor.User != null
                               ? $"{src.Instructor.User.FirstName} {src.Instructor.User.LastName}"
                               : "بدون استاد"))
                .ForMember(dest => dest.SemesterName,
                           opt => opt.MapFrom(src => src.Semester != null ? src.Semester.Name : "نامشخص"));

            CreateMap<Enrollment, EnrolledCourseDto>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade))
                 .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                 // خواندن نام درس از پرپرتی ارتباطی Course
                 .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : ""))
                 // خواندن تعداد واحد از پرپرتی ارتباطی Course
                 .ForMember(dest => dest.CourseUnit, opt => opt.MapFrom(src => src.Course != null ? src.Course.Unit.ToString() : "0"))
                // خواندن نام استاد در صورت وجود
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src =>
                          src.Course != null && src.Course.Instructor != null && src.Course.Instructor.User != null
                          ? $"{src.Course.Instructor.User.FirstName} {src.Course.Instructor.User.LastName}"
                              : "بدون استاد"));

            CreateMap<WorkflowRequest, WorkflowRequestDto>();
            CreateMap<Student, StudentProfileDto>();


        }

    }
}
