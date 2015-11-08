﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UniversityWebsite.Core;
using UniversityWebsite.Domain;
using UniversityWebsite.Services.Helpers;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    public interface IMenuService
    {
        MenuDto GetMainMenu(string lang);
        MenuDto GetMainMenuCached(string lang);
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
        public MenuDto GetMainMenu(string lang)
        {
            var menu = _menus.Include(m=>m.Items).Include(m=>m.Language).Single(m => m.Language.CountryCode == lang);
            var returnMenu = new MenuDto{MenuItems = new List<MenuItemDto>()};
            foreach (var page in menu.Items.OrderBy(mi=>mi.Id))//todo
                returnMenu.MenuItems.Add(new MenuItemDto { Text = page.Title, Href = page.UrlName });
            return returnMenu;
        }
        public MenuDto GetMainMenuCached(string lang)
        {
            MenuDto mainMenu = CacheHelper.GetOrInvoke<MenuDto>(
                "MainMenu"+lang,
                () => GetMainMenu(lang),
                TimeSpan.FromSeconds(10));//Todo
            return mainMenu;
        }
    }
}
