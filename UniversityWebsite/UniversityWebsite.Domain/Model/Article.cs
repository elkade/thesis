using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    public abstract class Article
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "text")]
        public string Content { get; set; }

        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        public virtual Subject Subject { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        public Article()
        {
            PublishDate = DateTime.Now;
            Content = String.Empty;
        }
    }

    public class News : Article
    {
        [Required]
        [StringLength(64)]
        public string Header { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

    }

    public class Syllabus : Article
    {
        [Key, ForeignKey("Subject")]
        public new int Id { get; set; }
    }

    public class Schedule : Article
    {
        [Key, ForeignKey("Subject")]
        public new int Id { get; set; }
    }
}
