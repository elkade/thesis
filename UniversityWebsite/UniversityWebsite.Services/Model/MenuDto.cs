using System.Collections.Generic;

namespace UniversityWebsite.Services.Model
{
    public class MenuDto
    {
        public List<MenuItemDto> MenuItems = new List<MenuItemDto>();
    }

    public class MenuItemDto
    {
        public string Text { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
    }
}
