using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversityWebsite.Domain.Enums;

namespace UniversityWebsite.Domain.Model
{
    [Table("Article")]
    public class Article
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(64)]
        public string Header { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; }
        [Required]
        public virtual User Author { get; set; }
        [Required]
        public DateTime PublishDate { get; set; }
        [Required]
        public ArticleStatus Status { get; set; }
    }
}