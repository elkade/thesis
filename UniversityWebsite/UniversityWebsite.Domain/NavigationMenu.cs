using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain
{
    public class NavigationMenu
    {
        [Key]
        public int Id { get; set; }

        public string CountryCode { get; set; }
        public ICollection<Page> Items { get; set; }
    }
}