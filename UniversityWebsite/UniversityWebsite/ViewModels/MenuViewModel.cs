using System.Collections.Generic;
using System.Linq;
using UniversityWebsite.Services;

namespace UniversityWebsite.ViewModels
{
    public class MenuViewModel
    {
        public MenuViewModel(Menu menuData)
        {
            MenuItems = menuData.MenuItems.Select(
                mi=>new MenuItemViewModel{Href = mi.Href, Text = mi.Text, Title = mi.Title}).ToList();
        }

        public List<MenuItemViewModel> MenuItems = new List<MenuItemViewModel>();
    }

    public class MenuItemViewModel
    {
        public string Text { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
    }
}