using System.ComponentModel.DataAnnotations;
namespace UniversityWebsite.Domain.Model
{
    public class StaticPage : Item
    {
        public string Url;

        [Key]
        public int Id { get; set; }
        public override string Link
        {
            get { return Url; }
        }
    }
}
