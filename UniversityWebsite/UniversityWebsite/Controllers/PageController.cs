using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using UniversityWebsite.Model;
using UniversityWebsite.Model.Menu;
using UniversityWebsite.Model.Page;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

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
        private readonly IDictionaryService _dictionaryService;

        /// <summary>
        /// Tworzy nową instancję kontrolera.
        /// </summary>
        /// <param name="pageService">Serwis zarządzający stronami systemu</param>
        /// <param name="languageService">Serwis zarządzający językami systemu</param>
        /// <param name="menuService">Serwis zarządzający menu systemu</param>
        /// <param name="dictionaryService">Serwis zarządzający tłumaczeniem fraz w systemie</param>
        public PageController(IPageService pageService, ILanguageService languageService, IMenuService menuService, IDictionaryService dictionaryService)
        {
            _pageService = pageService;
            _languageService = languageService;
            _menuService = menuService;
            _dictionaryService = dictionaryService;
        }
        /// <summary>
        /// Zwraca stronę o podanym UrlName
        /// </summary>
        /// <param name="name">UrlName strony</param>
        /// <returns>Obiekt widoku</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string name)
        {
            var page = _pageService.FindPage(name);
            string countryCode;
            PageViewModel pageVm;
            IEnumerable<PageMenuItem> siblings;
            LanguageMenuViewModel languageMenu;
            if (page == null)
            {
                Response.StatusCode = 404;
                countryCode = (string) HttpContext.Session[Consts.SessionKeyLang];
                pageVm = new PageViewModel
                {
                    Title = _dictionaryService.GetTranslationCached("notFoundTitle", countryCode),
                    Content = _dictionaryService.GetTranslationCached("notFoundText", countryCode)
                };
                siblings = _pageService.GetParentlessPagesWithChildren(countryCode).ToList();
                languageMenu = null;
            }
            else
            {
                countryCode = page.CountryCode;
                pageVm = Mapper.Map<PageViewModel>(page);
                siblings = _pageService.FindSiblingsWithChildren(name).ToList();
                HttpContext.Session[Consts.SessionKeyLang] = countryCode;
                SetCookie(Consts.CookieKeyLang, countryCode, HttpContext.Response);

                var languages = _languageService.GetLanguagesCached().ToList();

                var translations = _pageService.GetTranslations(name).ToList();

                var query = from language in languages
                            where language.CountryCode != countryCode
                            join page1 in translations on language.CountryCode equals page1.CountryCode into gj
                            from page2 in gj.DefaultIfEmpty()
                            select new LanguageMenuItemViewModel
                            {
                                CountryCode = language.CountryCode,
                                Text = language.Title,
                                Href = page2 == null ? null : page2.UrlName
                            };
                languageMenu = new LanguageMenuViewModel
                {
                    Current = languages.Single(l => l.CountryCode == countryCode).Title,
                    Items = query.ToList()
                };
            }

            var mainMenu = _menuService.GetMainMenuCached(countryCode);

            pageVm.NavMenu = new NavMenuVm {Items = Mapper.Map<List<PageMenuItemVm>>(siblings), IsTopLevel = page==null||page.Parent.Id==0};


            ViewData[Consts.MainMenuKey] = new MainMenuViewModel(
                mainMenu.Items,
                languageMenu
                );
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