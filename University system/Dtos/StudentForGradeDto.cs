namespace University_system.Dtos
{
    public class StudentForGradeDto
    {
        public int EnrollmentId { get; set; } 
        public int StudentId { get; set; }
        public string StudentNumber { get; set; }
        public string StudentName { get; set; }
        public double? CurrentGrade { get; set; } 
    }
}
