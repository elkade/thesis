using System.Collections.Generic;
using System.Linq;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.ViewModels
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