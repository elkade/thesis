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
        public virtual ICollection<Article> News { get; set; }
        public virtual Article Syllabus { get; set; }
        public virtual Article Schedule { get; set; }
        public virtual ICollection<File> Files { get; set; } 
        public virtual ICollection<Teacher> Teachers { get; set; } 
        public virtual ICollection<Student> Students { get; set; } 
    }
}