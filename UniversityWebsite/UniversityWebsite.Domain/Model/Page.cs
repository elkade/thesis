using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    public class Page
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(64)]
        public string Title { get; set; }
        [Required]
        [StringLength(64)]
        public string UrlName { get; set; }
        [Column(TypeName = "text")]
        public string Content { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public DateTime LastUpdateDate { get; set; }
        [Required]
        [ForeignKey("Language")]
        public string CountryCode { get; set; }
        public Language Language { get; set; }
        [Required]
        [ForeignKey("PageGroup")]
        public int GroupId { get; set; }
        public PageGroup Group { get; set; }
        [ForeignKey("Page")]
        public int ParentId { get; set; }
        public virtual Page Parent { get; set; }
    }
}