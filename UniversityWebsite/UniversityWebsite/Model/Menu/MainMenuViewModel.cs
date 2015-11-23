using System.Collections.Generic;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Model.Menu
{
    public class MainMenuViewModel
    {
        public List<MenuItemDto> MainMenuItems { get; set; }
        public LanguageMenuViewModel LanguageMenu { get; set; }

        public MainMenuViewModel(List<MenuItemDto> mainMenuItems, LanguageMenuViewModel languageMenu)
        {
            MainMenuItems = mainMenuItems;
            LanguageMenu = languageMenu;
        }
    }
}