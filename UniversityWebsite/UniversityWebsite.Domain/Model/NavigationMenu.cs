using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    public class NavigationMenu
    {
        [Key]
        public int Id { get; set; }

        public virtual Language Language { get; set; }
        public virtual ICollection<Page> Items { get; set; }
    }
}