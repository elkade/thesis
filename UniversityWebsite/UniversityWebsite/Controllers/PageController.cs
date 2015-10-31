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
            var page = _pageService.FindPage(pageName);
            if (page == null)
                return View(new PageViewModel{Name = "NotFound"});
            var pageVm = new PageViewModel { Name = page.Title, Language = page.CountryCode};

            ViewBag.Language = page.CountryCode;
            ViewBag.Title = page.Title;

            return View(pageVm);
        }
    }
}