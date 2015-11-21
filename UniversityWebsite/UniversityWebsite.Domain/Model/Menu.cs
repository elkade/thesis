using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual ICollection<MenuItem> Items { get; set; }
    }
}