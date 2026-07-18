namespace University_system.Dtos
{
    public class WorkflowTemplateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool RequiresStaffApproval { get; set; }
    }
}
