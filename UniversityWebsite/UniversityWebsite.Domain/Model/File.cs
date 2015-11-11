using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    public class File 
    {
        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public DateTime UploadDate { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}

