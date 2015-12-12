using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Api.Model.Teaching
{
    public class SubjectPut
    {
        [Required, Range(0, int.MaxValue)]
        public int Id { get; set; }

        [Required, StringLength(64)]
        public string Name { get; set; }

        [Required, Range(1, 10)]
        public int Semester { get; set; }

        [Required]
        public Article Syllabus { get; set; }

        [Required]
        public Article Schedule { get; set; }

    }
}