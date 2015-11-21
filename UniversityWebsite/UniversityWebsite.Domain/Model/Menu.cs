using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        public string CountryCode { get; set; }
        [Required]
        [ForeignKey("CountryCode")]
        public virtual Language Language { get; set; }
        [Required]
        public virtual ICollection<MenuItem> Items { get; set; }
    }
}