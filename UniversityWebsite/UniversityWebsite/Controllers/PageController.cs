using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using UniversityWebsite.Model;
using UniversityWebsite.Model.Menu;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Exceptions;

namespace UniversityWebsite.Controllers
{
    [Authorize]
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly ILanguageService _languageService;
        private readonly IMenuService _menuService;

        public PageController(IPageService pageService, ILanguageService languageService, IMenuService menuService)
        {
            _pageService = pageService;
            _languageService = languageService;
            _menuService = menuService;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string name)
        {
            var page = _pageService.FindPage(name);
            if (page == null)
                throw new NotFoundException("Nie znaleziono strony: " + name);

            HttpContext.Session[Consts.SessionKeyLang] = page.CountryCode;
            SetCookie(Consts.CookieKeyLang, page.CountryCode, HttpContext.Response);
            var mainMenu = _menuService.GetMainMenuCached(page.CountryCode);

            var pageVm = Mapper.Map<PageViewModel>(page);

            var languages = _languageService.GetLanguagesCached().ToList();

            var translations = _pageService.GetTranslations(name).ToList();

            var query = from language in languages
                        where language.CountryCode != page.CountryCode
                        join page1 in translations on language.CountryCode equals page1.CountryCode into gj
                        from page2 in gj.DefaultIfEmpty()
                        select new LanguageMenuItemViewModel
                        {
                            CountryCode = language.CountryCode,
                            Text = language.Title,
                            Href = page2 == null ? null : page2.UrlName
                        };
            ViewData[Consts.MainMenuKey] = new MainMenuViewModel(
                mainMenu.MenuItems,
                new LanguageMenuViewModel
                {
                    Current = languages.Single(l => l.CountryCode == page.CountryCode).Title,
                    Items = query.ToList()
                });
            ViewBag.Title = page.Title;
            return View(pageVm);
        }
        private static void SetCookie(string key, string value, HttpResponseBase response)
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