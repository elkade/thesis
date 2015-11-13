using System.Web.Mvc;
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
            return View(PageService.GetHomeTiles(Lang));
        }
	}
}