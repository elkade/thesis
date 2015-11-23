using System.Collections.Generic;

namespace UniversityWebsite.Model.Menu
{
    public class MenuViewModel
    {
        public List<MenuItemViewModel> MenuItems = new List<MenuItemViewModel>();
    }

    public class MenuItemViewModel
    {
        public string Text { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
    }
}