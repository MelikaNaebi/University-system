using AutoMapper;
using University_system.Dtos;
using University_system.Interface_Service;
using University_system.Interfaces;

namespace University_system.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AvailableCourseDto>> GetAvailableCoursesAsync(int semasterId)
        {
            var courses= await _courseRepository.GetCoursesByInstrctorIdAndSemasterIdAsync(semasterId);

            var courseDtos= _mapper.Map<IEnumerable<AvailableCourseDto>>(courses);

            return courseDtos;
        }
    }
}
