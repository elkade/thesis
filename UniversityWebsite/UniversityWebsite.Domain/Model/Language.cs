using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    public class Language
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(8)]
        public string CountryCode { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
    }
}
