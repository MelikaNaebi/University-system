using System.Collections.Generic;

namespace University_system.Dtos
{
    public class SemesterTranscriptDto
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } 

        public double SemesterGpa { get; set; } 
        public int SemesterCredits { get; set; } 
        public int TotalUnitsPassed { get; set; } 
        public string TermStatus { get; set; } 

        public IEnumerable<EnrolledCourseDto> Courses { get; set; }
    }
}