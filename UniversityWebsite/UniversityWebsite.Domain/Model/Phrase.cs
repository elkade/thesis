using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    public class Phrase
    {
        [Key, Column(Order = 0)]
        public int GroupId { get; set; }
        [Key, Column(Order = 1), ForeignKey("Language")]
        public int Language_Id { get; set; }
        [Required]
        public virtual Language Language { get; set; }
        [Required]
        [StringLength(512)]
        public string Text { get; set; }
    }
}
