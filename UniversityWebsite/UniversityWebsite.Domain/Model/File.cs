using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    public class File 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(256)]
        public string FileName { get; set; }
        [Required]
        [StringLength(128)]
        public string ContentType { get; set; }
        [Required]
        public byte[] Content { get; set; }
        [Required]
        public DateTime UploadDate { get; set; }
        public virtual User User { get; set; }
    }
}

