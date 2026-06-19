namespace University_system.Models
{
    public class Semaster
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsCurrent { get; set; }

        public ICollection<Course> Courses { get; set; }
        public ICollection<WorkflowRequest> WorkflowRequests { get; set; }
    }
}

    