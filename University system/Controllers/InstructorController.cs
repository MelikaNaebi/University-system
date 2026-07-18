using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University_system.Data;
using University_system.Dtos;
using University_system.Interface_Service;

namespace University_system.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly DataContext _context;
     
        public InstructorController( ICourseService courseService, DataContext context , IEnrollmentService enrollmentService )
        {
            _courseService = courseService;
            _context = context;
            _enrollmentService = enrollmentService;
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetSemesterCourses()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "شناسه کاربری در توکن یافت نشد.");
            }

            try
            {
                // ۲. پیدا کردن مستقیم استاد با کانتکست (همان کدی که تست کردی و جواب داد)
                var instructor = await _context.Instructors.FirstOrDefaultAsync(i => i.UserId == userId);

                if (instructor == null)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "حساب کاربری استاد یافت نشد.");
                }

                // ۳. گرفتن درس‌ها با آی‌دی واقعی استاد (عدد ۸) و فرستادن به سرویس
                var courses = await _courseService.GetInstructorCoursesAsync(instructor.Id);

                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("courses/{coursrId}/students")]
        public async Task<IActionResult> GetCoursesStudents([FromRoute] int coursrId)
        {
            try
            {
                var students = await _enrollmentService.GetCourseStudentsbycourseAsync(coursrId);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("submit-grade")]
        public async Task<IActionResult> SubmitGrade([FromBody] SubmitGradeDto submitGradeDto)
        {
            if (submitGradeDto.Grade < 0 || submitGradeDto.Grade > 20)
            {
                return BadRequest("نمره وارد شده باید بین 0 و 20 باشد.");
            }
            var isSuccess = await _enrollmentService.SubmitOrUpdateGradeByInstructorAsync(submitGradeDto.StudentId, submitGradeDto.CourseId, submitGradeDto.Grade);

            if (!isSuccess)
            {
                return NotFound("رکوردی برای این دانشجو در این درس یافت نشد.");
            }

            return Ok(new { message = "نمره با موفقیت ثبت و ویرایش شد." });

        }


    
    }
}
