using System.Collections.Generic;

namespace UniversityWebsite.Services.Model
{
    public class MenuDto
    {
        public List<MenuItemDto> MenuItems = new List<MenuItemDto>();
    }

    public class MenuItemDto
    {
        public string PageName { get; set; }
        public string Title { get; set; }
    }
}
