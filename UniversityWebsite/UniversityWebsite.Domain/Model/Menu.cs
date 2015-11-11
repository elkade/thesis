using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual Language Language { get; set; }
        public virtual ICollection<Page> Items { get; set; }
    }
}