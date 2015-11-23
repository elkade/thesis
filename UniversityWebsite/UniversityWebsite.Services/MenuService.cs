using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using UniversityWebsite.Core;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Helpers;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    public interface IMenuService
    {
        MenuDto GetMainMenu(string countryCode);
        MenuDto GetMainMenuCached(string lang);
        IEnumerable<MenuDto> GetAll();
        //List<MenuItemDto> GetMainMenuItemsCached();
        MenuDto UpdateMenu(MenuDto menu);
    }
    public class MenuService : IMenuService
    {
        private readonly IDomainContext _context;
        public MenuService(IDomainContext context)
        {
            _context = context;
        }
        public MenuDto GetMainMenu(string countryCode)
        {
            var menu = _context.Menus.SingleOrDefault(m => m.CountryCode == countryCode);
            if (menu == null) return new MenuDto();
            var items = _context.MenuItems
                .Where(mi=>mi.MenuId == menu.Id)
                .OrderBy(mi=>mi.Order)
                .ProjectTo<MenuItemDto>();
            return new MenuDto{MenuItems = items.ToList()};
        }
        public MenuDto GetMainMenuCached(string lang)
        {
            MenuDto mainMenu = CacheHelper.GetOrInvoke<MenuDto>(
                "MainMenu"+lang,
                () => GetMainMenu(lang),
                TimeSpan.FromSeconds(10));//Todo
            return mainMenu;
        }

        public IEnumerable<MenuDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public MenuDto UpdateMenu(MenuDto menu)
        {
            throw new NotImplementedException();
            //var dbMenu = _context.Menus.SingleOrDefault(m => m.CountryCode == menu.CountryCode);
            //if(dbMenu==null)
            //    throw new NotFoundException("No such menu in db. CountryCode: "+menu.CountryCode);
            //dbMenu.Items = menu.MenuItems.Select()
        }
    }
}
