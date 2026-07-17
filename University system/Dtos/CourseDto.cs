namespace University_system.Dtos
{
   
   
        public class CourseDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Unit { get; set; }
            public int Capacity { get; set; }
            public int EnrolledCount { get; set; }
            public int SemasterId { get; set; }
            public int InstructorId { get; set; }
        }
    }


