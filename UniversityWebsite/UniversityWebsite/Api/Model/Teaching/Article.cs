using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Api.Model.Teaching
{
    public class Article
    {
        [Required]
        public string Content { get; set; }
    }
}