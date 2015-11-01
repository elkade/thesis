using System.Web.Mvc;
using UniversityWebsite.Services;
using UniversityWebsite.ViewModels;

namespace UniversityWebsite.Controllers
{
    public class PageController : Controller
    {
        private IPageService _pageService;
        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }
        [HttpGet]
        public ActionResult Index(string pageName)
        {
            //todo ustawić cookie
            var page = _pageService.FindPage(pageName);
            if (page == null)
                return View(new PageViewModel{Name = "NotFound"});
            var pageVm = new PageViewModel { Name = page.Title, Language = page.Language.CountryCode};

            ViewBag.Language = page.Language.CountryCode;
            ViewBag.PageId = page.Id;
            ViewBag.Title = page.Title;

            return View(pageVm);
        }
    }
}