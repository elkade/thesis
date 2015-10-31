using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.Caching;
using UniversityWebsite.Core;
using UniversityWebsite.Domain;

namespace UniversityWebsite.Services
{
    public interface IMenuService
    {
        Menu GetMainMenu(int langId);
    }
    public class MenuService : IMenuService
    {
        protected IDomainContext _context;
        protected IDbSet<NavigationMenu> _menus;
        public MenuService(IDomainContext context)
        {
            _context = context;
            _menus = _context.Menus;
        }
        public Menu GetMainMenu(int langId)
        {
            return new Menu { MenuItems = new List<MenuItem>
            {
                new MenuItem{Href = "abc", Text = "Home", Title = "Strona główna"},
                new MenuItem{Href = "abc", Text = "Dydaktyka", Title = ""},
                new MenuItem{Href = "abc", Text = "Badania", Title = ""},
                new MenuItem{Href = "abc", Text = "Forum", Title = ""},
                new MenuItem{Href = "abc", Text = "Kontakt", Title = ""},
            }};
        }
        public Menu GetMainMenuCached(int langId)
        {
            Menu mainMenu = (Menu)MemoryCache.Default.Get("MainMenu");
            if (mainMenu == null)
                return new Menu {};
            return mainMenu;
        }
    }

    public class Menu
    {
        public List<MenuItem> MenuItems = new List<MenuItem>();
    }

    public class MenuItem
    {
        public string Text { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
    }
}
