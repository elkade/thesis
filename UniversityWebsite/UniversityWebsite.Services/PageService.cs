using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UniversityWebsite.Core;
using UniversityWebsite.Domain;

namespace UniversityWebsite.Services
{
    public interface IPageService
    {
        Page FindPage(string pageName);
        IEnumerable<Page> GetTranslations(int pageId);
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
            var page = new Page
            {
                //CountryCode = "32424",
                LangGroup = 1,
                Title = "test",
                Parent = null,
            };
            //page = _context.Pages.Add(page);
            //_context.SaveChanges();

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
    }
}
