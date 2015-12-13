using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using UniversityWebsite.Model.Menu;
using UniversityWebsite.Model.Page;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Exceptions;

namespace UniversityWebsite.Controllers
{
    /// <summary>
    /// Zawiera akcje dotyczące wyświetlania strony w systemie.
    /// </summary>
    [Authorize]
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly ILanguageService _languageService;
        private readonly IMenuService _menuService;
        /// <summary>
        /// Tworzy nową instancję kontrollera.
        /// </summary>
        /// <param name="pageService"></param>
        /// <param name="languageService"></param>
        /// <param name="menuService"></param>
        public PageController(IPageService pageService, ILanguageService languageService, IMenuService menuService)
        {
            _pageService = pageService;
            _languageService = languageService;
            _menuService = menuService;
        }
        /// <summary>
        /// Zwraca stronę o podanym UrlName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string name)
        {
            var page = _pageService.FindPage(name);
            var siblings = _pageService.FindSiblingsWithChildren(name).ToList();
            if (page == null)
                throw new NotFoundException("Nie znaleziono strony: " + name);

            HttpContext.Session[Consts.SessionKeyLang] = page.CountryCode;
            SetCookie(Consts.CookieKeyLang, page.CountryCode, HttpContext.Response);
            var mainMenu = _menuService.GetMainMenuCached(page.CountryCode);

            var pageVm = Mapper.Map<PageViewModel>(page);

            pageVm.Siblings = Mapper.Map<List<PageMenuItemVm>>(siblings);

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
                mainMenu.Items,
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