using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityWebsite.Services;
using UniversityWebsite.ViewModels;
using UniversityWebsite.ViewModels.Layout;

namespace UniversityWebsite.Controllers
{
    public class BaseController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IPageService _pageService;

        public BaseController(IMenuService menuService, IPageService pageService)
        {
            _menuService = menuService;
            _pageService = pageService;
        }

        private string _lang;

        private const string CookieKeyLang = "lang";
        private const string DefaultLanguage = "pl";

        protected int PageId = 0;//Todo

        protected string Lang
        {
            get
            {
                return _lang;
            }
            set
            {
                _lang = value;
                SetCookie(CookieKeyLang, value);
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            UpdateLanguage();

            AddMenu();
            AddLanguageSwitcher();
        }

        private void UpdateLanguage()
        {
            if (_lang == null)
                _lang = GetCookie(CookieKeyLang);
            if (_lang == null)
                _lang = DefaultLanguage;//TODO
        }

        private void AddMenu()
        {
            var mainMenuData = _menuService.GetMainMenuCached(_lang);
            MenuViewModel menu = new MenuViewModel(mainMenuData);
            ViewData["menu"] = menu;
        }

        private void AddLanguageSwitcher()
        {
            if (PageId <= 0)
            {
                ViewData["langSw"] = new LanguageSwitcherViewModel();
                return;
            }

            var translations = _pageService.GetTranslations(PageId);

            LanguageSwitcherViewModel switcher = new LanguageSwitcherViewModel();

            switcher.Languages = translations.Where(t => t.Language.CountryCode != _lang).Select(
                t => new LanguageButtonViewModel { IsPage = true, CountryCode = t.Language.CountryCode, Page = t.UrlName }).ToList();
            ViewData["langSw"] = switcher;
        }

        public void SetCookie(string key, string value)
        {
            var encodedValue = HttpUtility.UrlEncode(value);
            var cookie = new HttpCookie(key, encodedValue)
            {
                HttpOnly = true,
            };
            Response.AppendCookie(cookie);
        }

        public string GetCookie(string key)
        {
            var cookie = Request.Cookies[key];
            if (cookie == null)
                return null;
            var decodedValue = HttpUtility.UrlDecode(cookie.Value);
            return decodedValue;
        }
    }
}