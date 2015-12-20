using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    public class File 
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [StringLength(256)]
        public string FileName { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        [Required]
        public DateTime UpdateDate { get; set; }

        public int Version { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User Uploader { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }

        public int? SubjectId { get; set; }
    }
}

