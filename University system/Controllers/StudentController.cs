using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using University_system.Dtos;
using University_system.Interface_Service;

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
        private readonly IWorkflowRequestService _workflowRequestService;
        private readonly ILogger<StudentController> _logger; 

        public StudentController(IStudentService studentService, ILogger<StudentController> logger, IWorkflowRequestService workflowRequestService, ICourseService courseService, IEnrollmentService enrollmentService)
        {
            _studentService = studentService;
            _enrollmentService = enrollmentService;
            _courseService = courseService;
            _workflowRequestService = workflowRequestService;
            _logger = logger;
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
        public async Task<IActionResult> GetSemesterTranscript(int semesterId)
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
            try
            {
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

        [HttpGet("my-workflow-requests")]
        public async Task<IActionResult> GetStudentRequests()
        {
            var studentIdClaim = User.FindFirst("StudentId")?.Value; 

            if (string.IsNullOrEmpty(studentIdClaim))
                return Unauthorized("کاربر شناسایی نشد.");

            int studentId = int.Parse(studentIdClaim);
            var requests = await _workflowRequestService.GetStudentRequestsAsync(studentId);
            return Ok(requests);
        }
        [HttpPost("submit-workflow-request")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateWorkflowRequestDto dto)
        {
            try
            {
                var studentIdClaim = User.FindFirst("StudentId")?.Value; 
                if (string.IsNullOrEmpty(studentIdClaim))
                    return Unauthorized(new { message = "کاربر شناسایی نشد." });

                int studentId = int.Parse(studentIdClaim);

                if (dto == null || string.IsNullOrEmpty(dto.Title))
                    return BadRequest(new { message = "اطلاعات درخواست ناقص است." });

                var success = await _workflowRequestService.CreateRequestAsync(studentId, dto.TemplateId, dto.Title, dto.Description);

                if (!success)
                    return BadRequest(new { message = "خطا در ثبت درخواست. لطفاً قالب انتخابی را بررسی کنید." });

                return Ok(new { message = "درخواست با موفقیت ثبت و وارد چرخه بررسی شد." });
            }
            catch (Exception ex)
            {
                // ثبت خطا در لاگ سرور برای بررسی شما
                _logger.LogError(ex, "خطا در هنگام ثبت درخواست در اکشن CreateRequest");

                // بازگرداندن یک پاسخ JSON استاندارد به کلاینت
                // این کار باعث می‌شود کلاینت به جای خطای SyntaxError، یک پاسخ قابل فهم دریافت کند
                return StatusCode(500, new { message = "خطای داخلی سرور رخ داده است.", details = ex.Message });
            }
        }

        [HttpGet("workflow-templates")]
        public async Task<IActionResult> GetAllTemplates()
        {
            var templates = await _workflowRequestService.GetAllTemplatesAsync();
            return Ok(templates);
        }
    }
}