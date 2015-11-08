using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        public string CountryCode { get; set; }
        public string Name { get; set; }
    }
}
