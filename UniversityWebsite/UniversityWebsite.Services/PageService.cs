using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    /// <summary>
    /// Serwis realizujący logikę biznesową dotyczącą stron systemu.
    /// </summary>
    public interface IPageService
    {
        /// <summary>
        /// Wyszukuje stronę o podanym UrlName.
        /// </summary>
        /// <param name="name">UrlName strony</param>
        /// <returns>Strona o podanym UrlName lub null</returns>
        PageDto FindPage(string name);
        /// <summary>
        /// Wyszukuje strony bedące rodzeństwem danej w drzewie stron oraz ich dzieci.
        /// </summary>
        /// <param name="name">UrlName danej strony</param>
        /// <returns>Rodzieństwo strony i dzieci rodzeństwa.</returns>
        IEnumerable<PageMenuItem> FindSiblingsWithChildren(string name);
        /// <summary>
        /// Wyszukuje stronę o podanym Id.
        /// </summary>
        /// <param name="id">Id szukanej strony.</param>
        /// <returns>Strona o podanym Id.</returns>
        PageDto FindPage(int id);
        /// <summary>
        /// Wyszukuje tłumaczenie strony o podanym UrlName w danym języku
        /// </summary>
        /// <param name="name">UrlName strony</param>
        /// <param name="countryCode">język tłumaczenia strony</param>
        /// <returns>tłumaczenie danej strony w danym języku lub null, gdy tłumaczenie nie istnieje.</returns>
        PageDto FindTranslation(string name, string countryCode);
        /// <summary>
        /// Wyszukuje tłumaczenie strony o podanym Id w danym języku
        /// </summary>
        /// <param name="id">Id strony</param>
        /// <param name="countryCode">język tłumaczenia strony</param>
        /// <returns>tłumaczenie danej strony w danym języku lub null, gdy tłumaczenie nie istnieje.</returns>
        PageDto FindTranslation(int id, string countryCode);
        /// <summary>
        /// Wyszukuje wszystkie tłumaczenia strony o danym UrlName.
        /// </summary>
        /// <param name="name">UrlName strony</param>
        /// <returns>Wyliczenie wszystkich tłumaczeń danej strony</returns>
        IEnumerable<PageDto> GetTranslations(string name);
        /// <summary>
        /// Wyszukuje strony, których Parent wskazuje na null.
        /// </summary>
        /// <param name="countryCode">Język, w którym mają zostać wyszukane strony.</param>
        /// <returns>Wyliczenie stron.</returns>
        IEnumerable<PageMenuItem> GetParentlessPagesWithChildren(string countryCode);
        /// <summary>
        /// Zwraca wszystkie strony serwisu.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PageDto> GetAll();
        /// <summary>
        /// Aktualizuje zawartość strony.
        /// </summary>
        /// <param name="page">Strona wraz z zawartością, która ma nadpisać obecną.</param>
        void UpdateContent(PageDto page);
        /// <summary>
        /// Dodaje nową stronę do systemu
        /// </summary>
        /// <param name="page">Dodawana strona.</param>
        /// <returns>Dodana strona z bazy danych.</returns>
        PageDto Add(PageDto page);
        /// <summary>
        /// Aktualizuje istniejącą stronę nadpisując jej pola zawartymi w <paramref name="page"/> niebędącymi
        /// </summary>
        /// <param name="page">dane strony, którymi zostaną nadpisane aktualne.</param>
        /// <returns>Zaktualizowana strona z bazy danych.</returns>
        PageDto UpdatePage(PageDto page);
        /// <summary>
        /// Usuwa stronę z serwisu.
        /// </summary>
        /// <param name="id">Id strony do usunięcia.</param>
        void Delete(int id);
        /// <summary>
        /// Usuwa grupę stron z serwisu.
        /// </summary>
        /// <param name="pageId">Id grupy stron do usunięcia.</param>
        void DeleteGroup(int pageId);//TODO transakcje
        /// <summary>
        /// Wyszukuje języki na które istnieją w systemie tłumaczenia danej strony.
        /// </summary>
        /// <param name="id">Id danej strony</param>
        /// <returns>Wyliczenie kodów językowych</returns>
        IEnumerable<string> GetTranslationsLanguages(int id);
    }
    /// <summary>
    /// Implementacja serwisu realizującego logikę biznesową dotyczącą stron systemu.
    /// </summary>
    public class PageService : IPageService
    {
        private IDomainContext _context;
        private const int SameTitlePagesMaxNumber = 100;
        /// <summary>
        /// Tworzy nową instancję serwisu.
        /// </summary>
        /// <param name="context"></param>
        public PageService(IDomainContext context = null)
        {
            _context = context ?? new DomainContext();
        }

        public PageDto FindPage(int id)
        {
            var pages = _context.Pages.Where(p => p.Id == id)
                .ProjectTo<PageDto>();
            return pages.SingleOrDefault();
        }
        public PageDto FindPage(string name)
        {
            var pages = _context.Pages.Where(p => String.Compare(p.UrlName, name, StringComparison.OrdinalIgnoreCase) == 0)
                .ProjectTo<PageDto>();
            return pages.SingleOrDefault();
        }

        public IEnumerable<PageMenuItem> FindSiblingsWithChildren(string name)
        {
            var page =
                _context.Pages.SingleOrDefault(p => String.Compare(p.UrlName, name, StringComparison.OrdinalIgnoreCase) == 0);
            if (page == null)
                throw new NotFoundException("Page with urlName: " + name);
            var siblings = _context
                .Pages
                .Where(p => p.ParentId == page.ParentId && p.CountryCode == page.CountryCode)
                .ToList();
            return from siblingBuf in siblings
                   let children = _context
                    .Pages
                    .Where(p => p.ParentId == siblingBuf.Id)
                    .Select(p => new PageMenuItem { Title = p.Title, UrlName = p.UrlName })
                    .ToList()
                   select new PageMenuItem
                       {
                           Title = siblingBuf.Title,
                           UrlName = siblingBuf.UrlName,
                           Children = children
                       };
        }

        public PageDto FindTranslation(int id, string countryCode)
        {
            var page = _context.Pages.SingleOrDefault(p => p.Id == id);
            if (page == null)
                throw new NotFoundException("Page o id: " + id);
            var foundPages = _context.Pages.Where(p => p.GroupId == page.GroupId && p.CountryCode == countryCode).ProjectTo<PageDto>();
            if (!foundPages.Any())
                throw new NotFoundException("Page o grupie: " + page.GroupId + " i id: " + id);
            return foundPages.SingleOrDefault();
        }
        public PageDto FindTranslation(string name, string countryCode)
        {
            var page = _context.Pages.SingleOrDefault(p => String.Compare(p.UrlName, name, StringComparison.OrdinalIgnoreCase) == 0);
            if (page == null)
                throw new NotFoundException("Page o nazwie: " + name);
            var foundPages = _context.Pages.Where(p => p.GroupId == page.GroupId && p.CountryCode == countryCode).ProjectTo<PageDto>();
            if (!foundPages.Any())
                throw new NotFoundException("Page o grupie: " + page.GroupId + " i nazwie: " + name);
            return foundPages.SingleOrDefault();
        }

        public IEnumerable<PageDto> GetTranslations(string name)
        {
            var page = _context.Pages.SingleOrDefault(p => p.UrlName == name);
            if (page == null)
                throw new NotFoundException("Page o nazwie: " + name);
            return _context.Pages.Where(p => p.GroupId == page.GroupId).ProjectTo<PageDto>();
        }

        public IEnumerable<PageMenuItem> GetParentlessPagesWithChildren(string countryCode)
        {
            var siblings = _context
                .Pages
                .Where(p => p.ParentId == null && p.CountryCode == countryCode)
                .ToList();
            return from siblingBuf in siblings
                    let children = _context
                    .Pages
                    .Where(p => p.ParentId == siblingBuf.Id)
                    .Select(p => new PageMenuItem { Title = p.Title, UrlName = p.UrlName })
                    .ToList()
                    select new PageMenuItem
                    {
                        Title = siblingBuf.Title,
                        UrlName = siblingBuf.UrlName,
                        Children = children
                    };
        }
        public IEnumerable<PageDto> GetAll()
        {
            return _context.Pages.ProjectTo<PageDto>();
        }
        public void UpdateContent(PageDto page)
        {
            var dbPage = _context
                .Pages
                .SingleOrDefault(p => p.UrlName == page.UrlName);
            if (dbPage == null)
                throw new NotFoundException("Page o nazwie: " + page.UrlName);
            dbPage.Content = page.Content;
            dbPage.LastUpdateDate = DateTime.Now;
            _context.Entry(dbPage).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void ValidateLanguageUniqueness(string countryCode, int groupId)
        {
            if (_context.Pages.Any(p => p.CountryCode == countryCode && p.GroupId == groupId))
                throw new PropertyValidationException("page.CountryCode", string.Format("Page o countryCode: {0} i groupId: {1} już istnieje", countryCode, groupId));
        }

        public PageDto Add(PageDto page)
        {
            Page newParent = null;
            if (page.Parent != null)
            {
                newParent = _context.Pages.SingleOrDefault(p => p.Id == page.Parent.Id);
                if (newParent == null)
                    throw new NotFoundException("Parent o kodzie: " + page.Parent.UrlName);
            }
            PageGroup group = null;
            if (!page.GroupId.HasValue)
                group = _context.PageGroups.Add(new PageGroup());
            else
            {
                ValidateLanguageUniqueness(page.CountryCode, page.GroupId.Value);
                group = _context.PageGroups.SingleOrDefault(g => g.Id == page.GroupId.Value);
                if (group == null)
                    throw new NotFoundException("PageGroup o id: " + page.GroupId.Value);
            }

            var language = _context.Languages.SingleOrDefault(l => l.CountryCode == page.CountryCode);
            if (language == null)
                throw new NotFoundException("Language o countryCode: " + page.CountryCode);
            Page newPage = new Page
            {
                Language = language,
                Parent = newParent,
                Content = page.Content,
                LastUpdateDate = DateTime.Now,
                CreationDate = DateTime.Now,
                Title = page.Title,
                UrlName = page.UrlName ?? PrepareUniqueUrlName(page.Title),
                Group = group,
                Description = page.Description
            };
            Page createdPage = _context.Pages.Add(newPage);
            _context.SaveChanges();
            return Mapper.Map<PageDto>(createdPage);
        }

        public string PrepareUniqueUrlName(string baseUrlName)
        {
            if (!_context.Pages.Any(p => p.UrlName == baseUrlName))
                return baseUrlName;
            for (int i = 2; i < SameTitlePagesMaxNumber; i++)
            {
                string bufName = baseUrlName + i;
                if (!_context.Pages.Any(p => p.UrlName == bufName))
                    return bufName;
            }
            throw new PropertyValidationException("page.UrlName", "Przekroczono liczbę stron o tym samym tytule.");
        }

        public PageDto UpdatePage(PageDto page)
        {
            var dbPage = _context
                .Pages
                .SingleOrDefault(p => p.Id == page.Id);
            if (dbPage == null)
                throw new NotFoundException("Page o id: " + page.Id);
            if (page.CountryCode != null)
            {
                if (dbPage.CountryCode != page.CountryCode || page.GroupId != dbPage.GroupId)
                    ValidateLanguageUniqueness(page.CountryCode, dbPage.GroupId);
                dbPage.CountryCode = page.CountryCode;

                dbPage.Language = _context.Languages.FirstOrDefault(l => l.CountryCode == dbPage.CountryCode);
                dbPage.Group = _context.PageGroups.FirstOrDefault(g => g.Id == dbPage.GroupId);
            }
            if (page.Parent != null)
            {
                var newParent = _context.Pages.SingleOrDefault(p => p.Id == page.Parent.Id);
                if (newParent == null)
                    throw new NotFoundException("Parent o kodzie: " + page.Parent.UrlName);
                dbPage.Parent = newParent;
            }
            if (page.Content != null)
                dbPage.Content = page.Content;
            dbPage.LastUpdateDate = DateTime.Now;
            if (page.Title != null)
                dbPage.Title = page.Title;
            if (page.Description != null)
                dbPage.Description = page.Title;
            if (page.UrlName != null)
            {
                if (!_context.Pages.Any(p => p.UrlName == page.UrlName && p.Id != page.Id))
                    dbPage.UrlName = page.UrlName;
                else throw new PropertyValidationException("page.UrlName", "Strona o podanym urlu już istnieje.");
            }
            else page.UrlName = PrepareUniqueUrlName(page.Title);
            _context.Entry(dbPage).State = EntityState.Modified;

            _context.SaveChanges();
            return Mapper.Map<PageDto>(dbPage);
        }

        public void Delete(int id)
        {
            Page page = _context.Pages.FirstOrDefault(p => p.Id == id);
            if (page == null)
                throw new NotFoundException("Page o id: " + id);
            _context.Entry(page).State = EntityState.Deleted;

            _context.SaveChanges();
        }

        public void DeleteGroup(int pageId)//TODO transakcje
        {
            Page page = _context.Pages.Include(p1 => p1.Group).SingleOrDefault(p2 => p2.Id == pageId);
            if (page == null)
                throw new NotFoundException("Page o id: " + pageId);

            var pagesToDelete = _context.Pages.Where(p => p.GroupId == page.GroupId);
            foreach (var dbPage in pagesToDelete)
            {
                _context.Entry(dbPage).State = EntityState.Deleted;
            }
            _context.Entry(page.Group).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public IEnumerable<string> GetTranslationsLanguages(int id)
        {
            Page page = _context.Pages.FirstOrDefault(p => p.Id == id);
            if (page == null)
                throw new NotFoundException("Page o id: " + id);
            return _context.Pages.Where(p => p.GroupId == page.GroupId).Select(p => p.CountryCode);
        }
    }
}
