using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Permissions;
using UniversityWebsite.Core;
using UniversityWebsite.Domain;

namespace UniversityWebsite.Services
{
    public interface IPageService
    {
        Page FindPage(string pageName);
        IEnumerable<Page> GetTranslations(int pageId);
        ICollection<Page> GetHomeTiles(string lang);
        ICollection<Page> GetAll();
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
            return _context.Pages.FirstOrDefault(
                p => System.String.Compare(p.UrlName, pageName, System.StringComparison.OrdinalIgnoreCase)==0);
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
    }
}
