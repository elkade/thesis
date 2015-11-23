using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        public int PageId { get; set; }
        [Required]
        [ForeignKey("PageId")]
        public virtual Page Page { get; set; }
        public string File { get; set; }

        public int MenuId { get; set; }
        //[Required]
        [ForeignKey("MenuId")]
        public virtual Menu Menu { get; set; }

        public int Order { get; set; }
    }
}
