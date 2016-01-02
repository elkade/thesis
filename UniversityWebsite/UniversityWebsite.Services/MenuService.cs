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
    /// <summary>
    /// Serwis realizujący logikę biznesową dotyczącą menu systemu.
    /// </summary>
    public interface IMenuService
    {
        /// <summary>
        /// Wyszukuje menu główne w danym języku.
        /// </summary>
        /// <param name="countryCode">Kod języka menu</param>
        /// <returns>Menu główne</returns>
        MenuDto GetMainMenu(string countryCode);
        /// <summary>
        /// Pobiera menu główne w danym języku z pamięci cache.
        /// Jeżeli menu nie znajduje się w pamięci, wyszukuje w bazie danych.
        /// </summary>
        /// <param name="countryCode">Kod języka menu</param>
        /// <returns>Menu główne</returns>
        MenuDto GetMainMenuCached(string countryCode);
        /// <summary>
        /// Wyszukuje grupę menu o podanym id.
        /// </summary>
        /// <param name="groupId">Id grupy</param>
        /// <returns>Wyliczenie menu</returns>
        IEnumerable<MenuDto> GetMenuGroup(int groupId);
        //List<MenuItemDto> GetMainMenuItemsCached();
        /// <summary>
        /// Nadpisuje elementy menu. 
        /// </summary>
        /// <param name="menu">Elementy menu do nadpisania</param>
        void UpdateMenuItems(MenuData menu);
        /// <summary>
        /// Dodaje stronę do menu kafelków, jeżeli nie ma w menu strony o podanym Id
        /// </summary>
        /// <param name="pageId">Id strony do dodania</param>
        void AddToTilesMenuIfNotExists(int pageId);
        /// <summary>
        /// Wyszukuje menu kafelków w podanym języku.
        /// </summary>
        /// <param name="countryCode">Kod języka</param>
        /// <returns>Wyliczenie kafelków</returns>
        IEnumerable<Tile> GetTilesMenu(string countryCode);
        /// <summary>
        /// Wyszukuje menu kafelków w podanym języku z pamięci cache.
        /// Jeżeli menu nie znajduje się w pamięci, wyszukuje w bazie danych.
        /// </summary>
        /// <param name="countryCode">Kod języka menu</param>
        /// <returns>Wyliczenie kafelków</returns>
        IEnumerable<Tile> GetTilesMenuCached(string countryCode);
        /// <summary>
        /// Id grupy menu kafelkowego w bazie danych.
        /// </summary>
        int TilesMenuGroupId { get; }
        /// <summary>
        /// Id grupy menu głównego w bazie danych.
        /// </summary>
        int MainMenuGroupId { get; }
    }
    /// <summary>
    /// Implementacja serwisu realizującego logikę biznesową dotyczącą menu systemu.
    /// </summary>
    public class MenuService : IMenuService
    {
        private readonly IDomainContext _context;
        /// <summary>
        /// Tworzy nową instancję serwisu.
        /// </summary>
        /// <param name="context"></param>
        public MenuService(IDomainContext context)
        {
            _context = context;
        }
        public int TilesMenuGroupId
        {
            get { return 2; }
        }
        public int MainMenuGroupId
        {
            get { return 1; }
        }
        public MenuDto GetMainMenu(string countryCode)
        {
            var menu = _context.Menus.SingleOrDefault(m => m.CountryCode == countryCode && m.GroupId == 1);
            if (menu == null) return new MenuDto();
            var items = _context.MenuItems
                .Where(mi => mi.MenuId == menu.Id)
                .OrderBy(mi => mi.Order)
                .ProjectTo<MenuItemDto>();
            return new MenuDto { Items = items.ToList() };
        }
        public MenuDto GetMainMenuCached(string countryCode)
        {
            MenuDto mainMenu = CacheHelper.GetOrInvoke<MenuDto>(
                string.Format(CacheKeys.MenuKey, MainMenuGroupId, countryCode),
                () => GetMainMenu(countryCode),
                TimeSpan.FromSeconds(10));//Todo
            return mainMenu;
        }

        public IEnumerable<MenuDto> GetMenuGroup(int groupId)
        {
            return _context.Menus.Where(m => m.GroupId == groupId).ProjectTo<MenuDto>();
        }

        public void UpdateMenuItems(MenuData menu)
        {
            _context.InTransaction(() =>
            {
                var dbMenu = _context.Menus.SingleOrDefault(m => m.GroupId == menu.GroupId && m.CountryCode == menu.CountryCode);
                if (dbMenu == null)
                    throw new NotFoundException("No such menu in db. MenuId: " + menu.MenuId);

                var itemsToDelete = dbMenu.Items.ToList();
                foreach (var item in itemsToDelete)
                    _context.MenuItems.Remove(item);

                foreach (var item in menu.Items)
                    dbMenu.Items.Add(
                        new MenuItem
                        {
                            Menu = dbMenu, 
                            Order = item.Order,  
                            Description = item.Description,
                            Title = item.Title,
                            Page = _context.Pages.Single(p => p.Id == item.PageId),
                            ImageUrl = item.ImageUrl
                        });

                _context.SaveChanges();

                CacheHelper.Remove(string.Format(CacheKeys.MenuKey, dbMenu.GroupId, dbMenu.CountryCode));
            });
        }

        public void AddToTilesMenuIfNotExists(int pageId)
        {
            if (_context.MenuItems.Any(mi => mi.PageId == pageId))
                return;
            var page = _context.Pages.Find(pageId);
            if (page == null)
                throw new NotFoundException("Page with pageId: " + pageId);
            var menu = _context.Menus.Single(m => m.GroupId == TilesMenuGroupId && m.CountryCode == page.CountryCode);
            menu.Items.Add(new MenuItem { Menu = menu, Page = page, Order = menu.Items.Max(m => m.Order) + 1 });//uwaga na przepełnienie
            _context.SaveChanges();

            CacheHelper.Remove(string.Format(CacheKeys.MenuKey, TilesMenuGroupId, page.CountryCode));
        }

        public IEnumerable<Tile> GetTilesMenuCached(string countryCode)
        {
            var mainMenu = CacheHelper.GetOrInvoke<List<Tile>>(
                string.Format(CacheKeys.MenuKey, TilesMenuGroupId, countryCode),
                () => GetTilesMenu(countryCode).ToList(),
                TimeSpan.FromSeconds(10));//Todo
            return mainMenu;
        }

        public IEnumerable<Tile> GetTilesMenu(string countryCode)
        {
            
            var menuItems = _context.Menus.Single(m => m.GroupId == TilesMenuGroupId && m.CountryCode == countryCode).Items;
            return menuItems.OrderByDescending(mi => mi.Order)
                .Select(mi =>
                    new Tile
                    {
                        Title = !String.IsNullOrWhiteSpace(mi.Title) ? mi.Title : mi.Page.Title,
                        UrlName = mi.Page.UrlName,
                        Description = !String.IsNullOrWhiteSpace(mi.Description) ? mi.Description : mi.Page.Description,
                        ImageUrl = mi.ImageUrl
                    });
        }
    }
}
