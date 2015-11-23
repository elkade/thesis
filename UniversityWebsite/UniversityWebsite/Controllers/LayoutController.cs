//using System.Web.Mvc;
//using UniversityWebsite.Services;

//namespace UniversityWebsite.Controllers
//{
//    public class LayoutController : Controller
//    {
//        private IMenuService _menuService;
//        private IPageService _pageService;
//        //private ILanguageService _languageService;

//        public LayoutController(IMenuService menuService, IPageService pageService)
//        {
//            _menuService = menuService;
//            _pageService = pageService;
//        }

//        //public ActionResult MainMenu(string lang)
//        //{
//        //    var mainMenuData = MenuService.GetMainMenuCached(lang);
//        //    MenuViewModel menu = new MenuViewModel(mainMenuData);
//        //    return PartialView("_MainMenu",menu);
//        //}

//        //public ActionResult LanguageSwitcher(string currentLang, int pageId)
//        //{
//        //    if (pageId <= 0) return PartialView("_LanguageSwitcher", new LanguageSwitcherViewModel());
            
//        //    var translations = _pageService.GetTranslations(pageId);

//        //    LanguageSwitcherViewModel switcher = new LanguageSwitcherViewModel();

//        //    switcher.Languages = translations.Where(t=>t.Language.CountryCode!=currentLang).Select(
//        //        t => new LanguageButtonViewModel {IsPage = true, CountryCode = t.Language.CountryCode, Page = t.UrlName}).ToList();

//        //    return PartialView("_LanguageSwitcher", switcher);
//        //}
//    }
//}