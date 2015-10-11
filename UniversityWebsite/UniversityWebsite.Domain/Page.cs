using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain
{
    public class Page
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int LangGroup { get; set; }
        public string CountryCode { get; set; }
        public virtual Page Parent { get; set; }
    }
}