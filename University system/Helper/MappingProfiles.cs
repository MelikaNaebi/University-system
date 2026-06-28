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
                           opt => opt.MapFrom(src => src.Semaster != null ? src.Semaster.Name : "نامشخص"));

            CreateMap<Course, EnrolledCourseDto>()
                .ForMember(dest => dest.InstructorName,
                           opt => opt.MapFrom(src => src.Instructor != null && src.Instructor.User != null
                               ? $"{src.Instructor.User.FirstName} {src.Instructor.User.LastName}"
                               : "بدون استاد"));

            CreateMap<WorkflowRequest, WorkflowRequestDto>();
            CreateMap<Student, StudentProfileDto>();


        }

    }
}
