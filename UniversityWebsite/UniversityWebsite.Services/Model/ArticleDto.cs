using System;

namespace UniversityWebsite.Services.Model
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public string Author { get; set; }
    }
}
