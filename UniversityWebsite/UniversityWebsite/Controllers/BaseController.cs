using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityWebsite.Domain;
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

        private string _lang = null;

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
                if (_lang != null)
                    return;
                _lang = value;
                SetCookie(CookieKeyLang, value);
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string lang = filterContext.HttpContext.Request.Params["language"];
            if (!string.IsNullOrEmpty(lang)) Lang = lang; //todo walidacja
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            UpdateLanguage();

            AddMenu();
            AddLanguageSwitcher();
        }

        private void UpdateLanguage()
        {
            if (_lang == null)
                _lang = GetCookie(CookieKeyLang);
            if (_lang == null)
                _lang = DefaultLanguage;//TODO walidacja cookie
        }

        private void AddMenu()
        {
            var mainMenuData = _menuService.GetMainMenuCached(_lang);
            MenuViewModel menu = new MenuViewModel(mainMenuData);
            ViewData["menu"] = menu;
        }

        private void AddLanguageSwitcher()
        {
            var switcher = new LanguageSwitcherViewModel();

            var languages = _menuService.GetLanguagesCached();

            var translations = _pageService.GetTranslations(PageId).ToList();

            var query = from language in languages
                        join page in translations on language.CountryCode equals page.Language.CountryCode into gj
                        from page2 in gj.DefaultIfEmpty()
                        select new LanguageButtonViewModel { IsPage = (page2 != null), CountryCode = language.CountryCode, Title = language.Name, UrlName = page2 == null ? null : page2.UrlName };
            switcher.Languages = query.ToList();

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