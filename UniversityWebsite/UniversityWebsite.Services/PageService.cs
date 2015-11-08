using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using UniversityWebsite.Core;
using UniversityWebsite.Domain;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    public interface IPageService
    {
        Page FindPage(string pageName);
        IEnumerable<Page> GetTranslations(int pageId);
        ICollection<Page> GetHomeTiles(string lang);
        ICollection<Page> GetAll(); void UpdateContent(Page page);
        void Add(Page page);
    }

    public class PageService : IPageService
    {
        private IDomainContext _context;

        public PageService(IDomainContext context = null)
        {
            _context = context ?? new DomainContext();
        }

        public Page FindPage(string pageName)
        {
            //Mapper.CreateMap<Page, PageDto>()
            //    .ForMember(dto => dto.CountryCode, conf => conf.MapFrom(p => p.Language.CountryCode))
            //    .ForMember(dto => dto.ParentUrlName, conf => conf.MapFrom(
            //        p => p.Parent==null ? null : p.Parent.UrlName));
            var pages = _context.Pages.Where(
                p => String.Compare(p.UrlName, pageName, StringComparison.OrdinalIgnoreCase) == 0);//.ProjectTo<PageDto>();
            return pages.FirstOrDefault();
        }

        public IEnumerable<Page> GetTranslations(int pageId)
        {
            var page = _context.Pages.SingleOrDefault(p => p.Id == pageId);
            if (page == null)
                return Enumerable.Empty<Page>();
            return _context.Pages.Include(p => p.Language).Where(p => p.LangGroup == page.LangGroup);
        }

        public ICollection<Page> GetHomeTiles(string lang)
        {
            return _context.Pages.Where(p => p.Parent == null && p.Language.CountryCode == lang).ToList();
        }
        public ICollection<Page> GetAll()
        {
            return _context.Pages.ToList();
        }
        public void UpdateContent(Page page)
        {
            var dbPage = _context
                .Pages
                .Include(p => p.Language)
                .Single(p => p.Title == page.Title && p.Language.CountryCode == page.Language.CountryCode);
            dbPage.Content = page.Content;
            _context.Entry(dbPage).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void AddTranslation(string translatedPageName, Page translationPage)
        {
            var translatedPage = _context.Pages.SingleOrDefault(p => p.UrlName == translatedPageName);
            if (translatedPage == null) return;
            int groupId = translatedPage.LangGroup;
            translationPage.LangGroup = groupId;
            _context.Pages.Add(translationPage);
        }

        public void Add(Page page)
        {
            _context.Pages.Add(page);
            _context.SaveChanges();
        }

        public void Delete(string pageName)
        {
            Page page = _context.Pages.FirstOrDefault(p => p.UrlName == pageName);
            _context.Entry(page).State = EntityState.Deleted;

            _context.SaveChanges();
        }
    }
}
