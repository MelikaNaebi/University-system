namespace University_system.Dtos
{
    public class CreateWorkflowRequestDto
    {
        public int TemplateId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
