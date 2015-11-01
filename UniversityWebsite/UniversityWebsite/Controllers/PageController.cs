using System.Web.Mvc;
using UniversityWebsite.Services;
using UniversityWebsite.ViewModels;

namespace UniversityWebsite.Controllers
{
    public class PageController : BaseController
    {
        private IPageService _pageService;
        public PageController(IMenuService menuService, IPageService pageService)
            : base(menuService, pageService)
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

            Lang = page.Language.CountryCode;
            PageId = page.Id;

            ViewBag.Title = page.Title;
            return View(pageVm);
        }
    }
}