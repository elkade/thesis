using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Semester { get; set; }
    }
}