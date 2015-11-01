using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UniversityWebsite.Core;
using UniversityWebsite.Domain;
using UniversityWebsite.Services.Helpers;

namespace UniversityWebsite.Services
{
    public interface IMenuService
    {
        Menu GetMainMenu(string lang);
        Menu GetMainMenuCached(string lang);
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
        public Menu GetMainMenu(string lang)
        {
            var menu = _menus.Include(m=>m.Items).Include(m=>m.Language).Single(m => m.Language.CountryCode == lang);
            var returnMenu = new Menu{MenuItems = new List<MenuItem>()};
            foreach (var page in menu.Items.OrderBy(mi=>mi.Id))//todo
                returnMenu.MenuItems.Add(new MenuItem { Text = page.Title, Href = page.UrlName });
            return returnMenu;
        }
        public Menu GetMainMenuCached(string lang)
        {
            Menu mainMenu = CacheHelper.GetOrInvoke<Menu>(
                "MainMenu"+lang,
                () => GetMainMenu(lang),
                TimeSpan.FromSeconds(10));//Todo
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
        public int Type { get; set; }
    }
}
