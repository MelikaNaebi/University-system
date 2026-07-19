using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_system.Dtos;
using University_system.Interface_Service;

namespace University_system.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly IWorkflowRequestService _workflowRequestService;

        public StaffController(IWorkflowRequestService workflowRequestService)
        {
            _workflowRequestService = workflowRequestService;
        }

        [HttpGet("workflow-requests-cartable")]
        public async Task<IActionResult> GetStaffCartable()
        {
            var requests = await _workflowRequestService.GetStaffCartableAsync();
            return Ok(requests);
        }

        [HttpPost("review-workflow-request")]
        public async Task<IActionResult> StaffReview([FromBody] ReviewWorkflowRequestDto dto)
        {
            var success = await _workflowRequestService.ReviewByStaffAsync(dto);
            if (!success) return BadRequest("درخواست یافت نشد یا وضعیت آن معتبر نیست.");

            string responseMessage = dto.IsApproved
                ? "درخواست تایید اولیه شد و به کارتابل مدیر گروه فرستاده شد."
                : "درخواست توسط کارشناس آموزش رد شد.";

            return Ok(new { message = responseMessage });
        }
    }
}