using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Helpers;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    public interface IMenuService
    {
        MenuDto GetMainMenu(string countryCode);
        MenuDto GetMainMenuCached(string lang);
        IEnumerable<MenuDto> GetMainMenuGroup();
        //List<MenuItemDto> GetMainMenuItemsCached();
        void UpdateMenuItems(MenuData menu);
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
            var menu = _context.Menus.SingleOrDefault(m => m.CountryCode == countryCode && m.GroupId==1);
            if (menu == null) return new MenuDto();
            var items = _context.MenuItems
                .Where(mi=>mi.MenuId == menu.Id)
                .OrderBy(mi=>mi.Order)
                .ProjectTo<MenuItemDto>();
            return new MenuDto{Items = items.ToList()};
        }
        public MenuDto GetMainMenuCached(string lang)
        {
            MenuDto mainMenu = CacheHelper.GetOrInvoke<MenuDto>(
                "MainMenu"+lang,
                () => GetMainMenu(lang),
                TimeSpan.FromSeconds(10));//Todo
            return mainMenu;
        }

        public IEnumerable<MenuDto> GetMainMenuGroup()
        {
            return _context.Menus.Where(m => m.GroupId == 1).ProjectTo<MenuDto>();
        }

        public void UpdateMenuItems(MenuData menu)
        {
            var dbMenu = _context.Menus.SingleOrDefault(m => m.GroupId == 1 && m.CountryCode == menu.CountryCode);
            if(dbMenu==null)
                throw new NotFoundException("No such menu in db. MenuId: " + menu.MenuId);

            var itemsToDelete = dbMenu.Items.ToList();
            foreach (var item in itemsToDelete)
                _context.MenuItems.Remove(item);

            foreach (var item in menu.Items)
                dbMenu.Items.Add(new MenuItem{Menu = dbMenu, Order = item.Order, Page = _context.Pages.Single(p=>p.Id==item.PageId)});

            _context.SaveChanges();
        }
    }
}
