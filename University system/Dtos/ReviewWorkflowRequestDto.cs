namespace University_system.Dtos
{
    public class ReviewWorkflowRequestDto
    {
        public int RequestId { get; set; }
        public bool IsApproved { get; set; }
        public string? Comment { get; set; }
    }
}
