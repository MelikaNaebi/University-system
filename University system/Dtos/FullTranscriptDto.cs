using System.Collections.Generic;

namespace University_system.Dtos
{
    public class FullTranscriptDto
    {
        public int StudentId { get; set; }

        public double TotalGpa { get; set; } 
        public int TotalCredits { get; set; } 
        public string OverallStatus { get; set; } 

        public IEnumerable<SemesterTranscriptDto> Semesters { get; set; }
    }
}