using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Semester Semester { get; set; }
        public ICollection<Article> News { get; set; }
        public Article Syllabus { get; set; }
        public Article Schedule { get; set; }
        public ICollection<File> Files { get; set; } 
        public virtual ICollection<Teacher> Teachers { get; set; } 
        public virtual ICollection<Student> Students { get; set; } 
    }
}