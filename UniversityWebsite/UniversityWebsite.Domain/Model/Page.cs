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
        public string CountryCode { get; set; }
        [Required]
        [ForeignKey("CountryCode")]
        public Language Language { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        [ForeignKey("GroupId")]
        public PageGroup Group { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Page Parent { get; set; }

        public int Order { get; set; }
        public string Description { get; set; }
    }
}