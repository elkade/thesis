
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain
{
    public class Phrase
    {
        [Key, Column(Order = 0)]
        public string Group { get; set; }
        [Key, Column(Order = 1), ForeignKey("Language")]
        public int Language_Id { get; set; }
        public virtual Language Language { get; set; }
        public string Text { get; set; }
    }
}
