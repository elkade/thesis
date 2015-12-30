using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    public class PageGroup
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<Page> Pages { get; set; } 
    }
}
