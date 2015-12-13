using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Api.Model.Teaching
{
    public class SubjectPost
    {
        [Required, StringLength(64)]
        public string Name { get; set; }

        [Required, Range(1,10)]
        public int Semester { get; set; }

        public Article Syllabus { get; set; }

        public Article Schedule { get; set; }
    }
}