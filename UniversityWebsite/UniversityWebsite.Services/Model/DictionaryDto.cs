using System.Collections.Generic;

namespace UniversityWebsite.Services.Model
{
    public class DictionaryDto
    {
        public string CountryCode { get; set; }
        public string Title { get; set; }
        public Dictionary<string, string> Words { get; set; } 
    }
}
