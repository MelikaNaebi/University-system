namespace University_system.Models
{
    public class Student
    {

        public int Id { get; set; }
        public string StudentNumber { get; set; }


        public string UserId { get; set; }
        public User User { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<WorkflowRequest> WorkflowRequests { get; set; }
    }
}
