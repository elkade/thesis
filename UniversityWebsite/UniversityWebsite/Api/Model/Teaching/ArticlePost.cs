using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Api.Model.Teaching
{
    public class ArticlePost
    {
        public string Content { get; set; }

        [Required]
        public int SubjectId { get; set; }
    }
}