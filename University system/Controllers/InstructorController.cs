using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_system.Interface_Service;

namespace University_system.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public InstructorController( ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("semester-courses")]
        public async Task<IActionResult> GetSemesterCourses()
        {
            var InstructorIdClaim = User.FindFirst("InstructorId")?.Value;

            if (string.IsNullOrEmpty(InstructorIdClaim))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "شما با حساب کاربری استاد وارد نشده‌اید و به این بخش دسترسی ندارید.");
            }
            if (!int.TryParse(InstructorIdClaim, out int instructorId))
            {
                return BadRequest("اطلاعات کاربری شما نامعتبر است. لطفاً دوباره وارد سیستم شوید.");
            }
            try
            {
                var courses = await _courseService.GetInstructorCoursesAsync(instructorId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
