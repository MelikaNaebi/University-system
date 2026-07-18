namespace University_system.Dtos
{
    public class StudentReportFilterDto
    {
        public string? StudentNumber { get; set; }
        public string? Name { get; set; }
        public string StatusFilter { get; set; } = "All"; 
        public int SemesterId { get; set; } 
    }
}
