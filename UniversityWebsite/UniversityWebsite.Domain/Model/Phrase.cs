using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    public class Phrase
    {
        [Key, Column(Order = 0)]
        public string Key { get; set; }
        [Key, Column(Order = 1)]
        public string CountryCode { get; set; }
        [Required]
        [StringLength(512)]
        public string Value { get; set; }
    }
}
