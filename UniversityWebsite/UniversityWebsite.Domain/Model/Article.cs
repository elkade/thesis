using System;
using System.ComponentModel.DataAnnotations.Schema;
using UniversityWebsite.Domain.Enums;

namespace UniversityWebsite.Domain.Model
{
    [Table("Article")]
    public class Article
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime PublishDate { get; set; }
        public ArticleStatus Status { get; set; }
    }
}