using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Api.Model.Teaching
{
    public class News
    {
        [Required]
        public string Header { get; set; }
        [Required]
        public string Content { get; set; }
    }
}