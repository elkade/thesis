using System.Linq;
using UniversityWebsite.Core;
using UniversityWebsite.Domain;

namespace UniversityWebsite.Services
{
    public interface IPageService
    {
        Page FindPage(string pageName);
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
                CountryCode = "32424",
                LangGroup = 1,
                Title = "test",
                Parent = null,
            };
            //page = _context.Pages.Add(page);
            //_context.SaveChanges();

            return _context.Pages.FirstOrDefault(
                p => System.String.Compare(p.UrlName, pageName, System.StringComparison.OrdinalIgnoreCase)==0);
        }
    }
}
