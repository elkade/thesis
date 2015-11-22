using System.Web;
using System.Web.Mvc;
using AutoMapper;
using UniversityWebsite.Model;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Exceptions;

namespace UniversityWebsite.Controllers
{
    [Authorize]
    public class PageController : BaseController
    {
        public IPageService PageService { get; set; }

        public PageController(IMenuService menuService, IPageService pageService)
            : base(menuService)
        {
            PageService = pageService;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string name)
        {
            var page = PageService.FindPage(name);
            if (page == null)
                throw new NotFoundException("Nie znaleziono strony: " + name);

            HttpContext.Session[Consts.SessionKeyLang] = page.CountryCode;
            SetCookie(Consts.CookieKeyLang, page.CountryCode, HttpContext.Response);

            var pageVm = Mapper.Map<PageViewModel>(page);

            ViewBag.Title = page.Title;
            return View(pageVm);

        }
        public void SetCookie(string key, string value, HttpResponseBase response)
        {
            var encodedValue = HttpUtility.UrlEncode(value);
            var cookie = new HttpCookie(key, encodedValue)
            {
                HttpOnly = true,
            };
            response.AppendCookie(cookie);
        }
    }
}