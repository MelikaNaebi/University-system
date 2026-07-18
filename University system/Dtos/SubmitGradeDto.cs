namespace University_system.Dtos
{
    public class SubmitGradeDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public double Grade { get; set; }
    }
}
