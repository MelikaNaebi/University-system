using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using University_system.Dtos;
using University_system.Interface_Service;
using University_system.Models;
using University_system.Services;

namespace University_system.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly ICourseService _courseService;

        public StudentController(IStudentService studentService, ICourseService courseService,IEnrollmentService enrollmentService)
        {
            _studentService = studentService;
            _enrollmentService = enrollmentService;
            _courseService = courseService;
        }
        [HttpGet("Fulltranscrip")]
        public async Task<IActionResult> GetFullTranscript()
        {
            var studentIdClaim = User.FindFirst("StudentId")?.Value;

            if (string.IsNullOrEmpty(studentIdClaim))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "شما با حساب کاربری دانشجویی وارد نشده‌اید و به این بخش دسترسی ندارید.");
            }
            if (!int.TryParse(studentIdClaim, out int studentId))
            {
                return BadRequest("اطلاعات کاربری شما نامعتبر است. لطفاً دوباره وارد سیستم شوید.");
            }
            var transcript = await _studentService.GetFullTranscriptAsync(studentId);
            return Ok(transcript);
        }

        [HttpGet("transcript/semester")]
        public async Task<IActionResult> GetSemesterTranscript( int semesterId)
        {
            var studentIdClaim = User.FindFirst("StudentId")?.Value;

            if (string.IsNullOrEmpty(studentIdClaim))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "شما با حساب کاربری دانشجویی وارد نشده‌اید و به این بخش دسترسی ندارید.");
            }
            if (!int.TryParse(studentIdClaim, out int studentId))
            {
                return BadRequest("اطلاعات کاربری شما نامعتبر است. لطفاً دوباره وارد سیستم شوید.");
            }
            var semesterTranscript = await _studentService.GetSemesterTranscriptAsync(studentId, semesterId);
            return Ok(semesterTranscript);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetStudentProfile()
        {
            var studentIdClaim = User.FindFirst("StudentId")?.Value;

            if (string.IsNullOrEmpty(studentIdClaim))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "شما با حساب کاربری دانشجویی وارد نشده‌اید و به این بخش دسترسی ندارید.");
            }
            if (!int.TryParse(studentIdClaim, out int studentId))
            {
                return BadRequest("اطلاعات کاربری شما نامعتبر است. لطفاً دوباره وارد سیستم شوید.");
            }
            var profile = await _studentService.GetStudentProfileAsync(studentId);
            if (profile == null) return NotFound(new { message = "دانشجو یافت نشد." });

            return Ok(profile);
        }

        [HttpGet("available")]

        public async Task<IActionResult> GetCoursesForEnroll()
        {
            try { 
            
            var courses = await _courseService.GetAvailableCoursesAsync();
                return Ok(courses);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("my-courses")]
        public async Task<IActionResult> GetMyCourses()
        {
            var studentIdClaim = User.FindFirst("StudentId")?.Value;

            if (string.IsNullOrEmpty(studentIdClaim))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "شما با حساب کاربری دانشجویی وارد نشده‌اید و به این بخش دسترسی ندارید.");
            }
            if (!int.TryParse(studentIdClaim, out int studentId))
            {
                return BadRequest("اطلاعات کاربری شما نامعتبر است. لطفاً دوباره وارد سیستم شوید.");
            }
            try
            {
                var courses = await _enrollmentService.GetStudentCoursesForActiveSemesterAsync(studentId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("enroll")]

        public async Task<IActionResult> Enroll([FromBody] EnrolledCourseDto enrolledCourseDto)
        {

            //////
            /////
            //var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            //Console.WriteLine("--- Claims in Token ---");
            //foreach (var claim in claims)
            //{
            //    Console.WriteLine($"{claim.Type}: {claim.Value}");
            //}
            ///////
            /////
            var studentIdClaim = User.FindFirst("StudentId")?.Value;

            if (string.IsNullOrEmpty(studentIdClaim))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "شما با حساب کاربری دانشجویی وارد نشده‌اید و به این بخش دسترسی ندارید.");
            }
            if (!int.TryParse(studentIdClaim, out int studentId))
            {
                return BadRequest("اطلاعات کاربری شما نامعتبر است. لطفاً دوباره وارد سیستم شوید.");
            }
            try
            {
                var result = await _enrollmentService.EnrollCourseAsync(enrolledCourseDto.CourseId, studentId);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

           
        }
    }
}
       

