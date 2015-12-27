using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversityWebsite.Domain.Model;

namespace UniversityWebsite.Domain
{
    public class TeacherSubject
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("TeacherId")]
        public virtual User Teacher { get; set; }
        public string TeacherId { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        public int SubjectId { get; set; }

    }
}
