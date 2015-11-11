using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    public class Phrase
    {
        [Key, Column(Order = 0)]
        public int GroupId { get; set; }
        [Key, Column(Order = 1), ForeignKey("Language")]
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public string Text { get; set; }
    }
}
