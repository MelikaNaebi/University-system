using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_system.Dtos;
using University_system.Interface_Service;

namespace University_system.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentHeadController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IWorkflowRequestService _workflowRequestService;
        private readonly IStudentService _studentService;

        public DepartmentHeadController(ICourseService courseService, IWorkflowRequestService workflowRequestService, IStudentService studentService)
        {
            _courseService = courseService;
            _studentService = studentService;
            _workflowRequestService = workflowRequestService;
        }

        [HttpPost("create-course")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourse course)
        {
            var isSuccess = await _courseService.AddNewCourseByAdmin(course.Title, course.Unit, course.Capacity, course.SemesterId, course.InstructorId);

            if (!isSuccess)
            {
                return BadRequest(new { message = "خطایی در ثبت درس جدید رخ داد. لطفاً اطلاعات ورودی را بررسی کنید یا مجدداً تلاش کنید." });
            }

            return Ok(new { message = "درس با موفقیت ایجاد شد." });
        }

        [HttpPost("students-report")]
        public async Task<IActionResult> GetStudentsReport([FromBody] StudentReportFilterDto filter)
        {
            try
            {
                var reportData = await _studentService.GetStudentsReportAsync(filter);
                return Ok(reportData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during generating student report: {ex.Message}");
                return StatusCode(500, new { message = "خطایی در تهیه گزارش دانشجویان رخ داده است." });
            }
        }

        [HttpGet("current-semester")]
        public async Task<IActionResult> GetCurrentSemester()
        {
            try
            {
                var semesterId = await _studentService.GetCurrentSemesterIdAsync();
                return Ok(new { SemesterId = semesterId });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "خطای داخلی سرور در دریافت ترم جاری.");
            }
        }

        [HttpGet("pending-workflow-requests")]
        public async Task<IActionResult> GetManagerCartable([FromQuery] int semesterId)
        {
            var requests = await _workflowRequestService.GetManagerCartableAsync(semesterId);
            return Ok(requests);
        }

        [HttpPost("review-workflow-request")]
        public async Task<IActionResult> ManagerReview([FromBody] ReviewWorkflowRequestDto dto)
        {
            var success = await _workflowRequestService.ReviewByManagerAsync(dto);
            if (!success) return BadRequest("درخواست یافت نشد یا وضعیت آن معتبر نیست.");

            string responseMessage = dto.IsApproved
                ? "درخواست تایید نهایی شد و چرخه فرآیند با موفقیت پایان یافت."
                : "درخواست توسط مدیر گروه رد شد.";

            return Ok(new { message = responseMessage });
        }

        [HttpPost("create-workflow-template")]
        public async Task<IActionResult> CreateTemplate([FromBody] CreateTemplateDto dto)
        {
            var success = await _workflowRequestService.CreateTemplateAsync(dto);
            if (!success) return BadRequest("خطا در ساخت قالب جدید.");

            return Ok(new { message = "قالب گردش کار جدید با موفقیت تعریف شد." });
        }
    }
}