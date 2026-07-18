namespace University_system.Dtos
{
    public class CreateCourse
    {
        public string Title { get; set; }

        public string Unit { get; set; }

        public int Capacity { get; set; }

        public int EnrolledCount { get; set; } = 0;

        public int InstructorId { get; set; }

        public int SemesterId { get; set; }


    }
}
