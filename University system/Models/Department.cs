namespace University_system.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? HeadInstructorId { get; set; }
        public Instructor HeadInstructor { get; set; }
        public ICollection<Instructor> Instructors { get; set; }
        public ICollection<Student> Students { get; set; }

    }
}
