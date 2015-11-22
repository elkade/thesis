using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using UniversityWebsite.Model;
using UniversityWebsite.Services;
using UniversityWebsite.ViewModels;
using UniversityWebsite.ViewModels.Layout;

namespace UniversityWebsite.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IMenuService MenuService;
        protected readonly IPageService PageService;
        protected readonly ILanguageService LanguageService;

        public BaseController(IMenuService menuService, IPageService pageService, ILanguageService languageService)
        {
            MenuService = menuService;
            PageService = pageService;
            LanguageService = languageService;
        }

        public BaseController()
        {
            
        }
        private string _lang = null;

        protected const string CookieKeyLang = "lang";
        private const string DefaultLanguage = "pl";

        protected string Lang
        {
            get
            {
                return _lang ?? DefaultLanguage; //TODO: I want default language but not null
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
            string lang = filterContext.HttpContext.Request.Unvalidated["language"];
            if (!string.IsNullOrEmpty(lang)) Lang = lang; //todo walidacja
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            UpdateLanguage();

            AddMenu();
            AddLanguageSwitcher();
            ViewData["lang"] = Lang;
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
            if (MenuService == null) return;
            var mainMenuData = MenuService.GetMainMenuCached(_lang);
            MenuViewModel menu = Mapper.Map<MenuViewModel>(mainMenuData);
            ViewData["menu"] = menu;
        }

        private void AddLanguageSwitcher()
        {
            //if (PageService == null) return;
            var switcher = new LanguageSwitcherViewModel();

            //var languages = LanguageService.GetLanguagesCached();

            //var translations = PageService.GetTranslations(0).ToList();

            //var query = from language in languages
            //            join page in translations on language.CountryCode equals page.CountryCode into gj
            //            from page2 in gj.DefaultIfEmpty()
            //            select new LanguageButtonViewModel
            //            {
            //                IsPage = (page2 != null), 
            //                CountryCode = language.CountryCode, 
            //                Title = language.Name, 
            //                UrlName = page2 == null ? null : page2.UrlName
            //            };
            //switcher.Languages = query.ToList();

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