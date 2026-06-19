namespace University_system.Models
{
    public class WorkflowRequest
    {


        public int Id { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; } 
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int SemesterId { get; set; }
        public Semaster Semaster { get; set; }
    }
}
