using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Model.Menu
{
    public class MenuItemPut
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int PageId { get; set; }
        public int Order { get; set; }
    }
}