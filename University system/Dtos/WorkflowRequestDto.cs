namespace University_system.Dtos
{
    public class WorkflowRequestDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CreatedAtPersian { get; set; }
        public string? StaffComment { get; set; }
        public string? ManagerComment { get; set; }

        public string TemplateName { get; set; }
    }
}
