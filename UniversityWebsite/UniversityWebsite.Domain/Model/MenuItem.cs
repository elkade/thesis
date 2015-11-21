using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
    }
}
