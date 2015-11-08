using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    public class Page
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string UrlName { get; set; }
        public string Content { get; set; }
        public int LangGroup { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public virtual Language Language { get; set; }
        public virtual Page Parent { get; set; }
    }
}