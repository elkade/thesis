using System.Linq;
using System.Web.Mvc;
using UniversityWebsite.Model;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IMenuService menuService, IPageService pageService, ILanguageService languageService)
            : base(menuService, pageService, languageService)
        {
            
        }

        public ActionResult Index()
        {
            var pages = PageService.GetParentlessPages(Lang);
            var tileList = pages.Select(p => new TileViewModel { Title = p.Title, UrlName = p.UrlName }).ToList();
            return View(tileList);
        }
	}
}