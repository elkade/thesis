using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Model
{
    public class NewLanguage
    {
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public List<KeyValuePair<string, string>> Words { get; set; } 
    }
}