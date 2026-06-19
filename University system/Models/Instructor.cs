namespace University_system.Models
{
    public class Instructor
    {

        public int Id { get; set; }
        public string TeacherNumber { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
