namespace University_system.Dtos
{
    public class CreateTemplateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool RequiresStaffApproval { get; set; }
    }
}
