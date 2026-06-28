namespace University_system.Dtos
{
    public class AvailableCourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Unit { get; set; }

        public int Capacity { get; set; }
        public int EnrolledCount { get; set; }
        public int RemainingCapacity => Capacity - EnrolledCount; 
        public string InstructorName { get; set; } 
        public string SemesterName { get;  set; }
    }
}
