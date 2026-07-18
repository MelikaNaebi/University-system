namespace University_system.Dtos
{
    public class StudentReportResponseDto
    {
        public int Id { get; set; }
        public string StudentNumber { get; set; }
        public string FullName { get; set; }
        public double GPA { get; set; }
        public string Status { get; set; }
    }
}
