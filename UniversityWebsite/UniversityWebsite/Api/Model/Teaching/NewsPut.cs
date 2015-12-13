using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Api.Model.Teaching
{
    public class NewsPut
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Header { get; set; }
        [Required]
        public string Content { get; set; }

    }
}