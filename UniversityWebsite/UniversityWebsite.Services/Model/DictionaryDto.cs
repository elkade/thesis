using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityWebsite.Services.Model
{
    public class DictionaryDto
    {
        public string CountryCode { get; set; }
        public string Title { get; set; }
        public List<KeyValuePair<string, string>> Words { get; set; } 
    }
}
