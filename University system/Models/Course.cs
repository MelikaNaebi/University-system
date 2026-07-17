namespace University_system.Models
{
    public class Course
    {

        public int Id { get; set; }

        public string  Title { get; set; }

        public string Unit { get; set; }

        public int Capacity { get; set; }     
        public int EnrolledCount { get; set; }

        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }

        public int SemesterId { get; set; }
        public Semester Semester { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
