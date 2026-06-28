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
        public async Task<StudentProfileDto> GetStudentProfileAsync(int studentId)
        {
            var StudentProfile = await _unitOfWork.Students.GetStudentWithUserAsync(studentId);

            if (StudentProfile == null) return null;

            return _mapper.Map<StudentProfileDto>(StudentProfile);

        }
    }
}
