using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Api.Model.Teaching
{
    public class NewsPost : ArticlePost
    {
        [Required]
        public string Header { get; set; }
    }
}