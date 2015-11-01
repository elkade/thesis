using System.Web.Mvc;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IMenuService menuService, IPageService pageService)
            : base(menuService, pageService)
        {
        }
        public ActionResult Index()
        {
            return View();
        }
	}
}