using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        [StringLength(64)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string UrlName { get; set; }

        public virtual Semester Semester { get; set; }
        [ForeignKey("Semester")]
        public int SemesterId { get; set; }

        public virtual ICollection<Article> News { get; set; }
        public virtual Article Syllabus { get; set; }
        public virtual Article Schedule { get; set; }
        public virtual ICollection<File> Files { get; set; } 
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<Student> Students { get; set; } 
    }
}