using System;

namespace University_system.Dtos
{
    public class EnrolledCourseDto
    {
        public int Id { get; set; }

        public double? Grade { get; set; }

        public int CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public string? CourseUnit { get; set; }

         public string? InstructorName { get; set; }
    }
}