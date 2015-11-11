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
        public string Header { get; set; }
        public string Content { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public DateTime PublishDate { get; set; }
        public ArticleStatus Status { get; set; }
    }
}