namespace University_system.Models
{
    public class WorkflowTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool RequiresStaffApproval { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
