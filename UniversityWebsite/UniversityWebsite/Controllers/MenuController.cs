//using System.Linq;
//using System.Web.Mvc;
//using AutoMapper;
//using UniversityWebsite.Model.Menu;
//using UniversityWebsite.Services;

//namespace UniversityWebsite.Controllers
//{
//    public class MenuController : Controller
//    {
//        private readonly IMenuService _menuService;

//        public MenuController(IMenuService menuService)
//        {
//            _menuService = menuService;
//        }

//        protected override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            string lang = (string)Session[Consts.SessionKeyLang];
//            var menu = _menuService.GetMainMenuCached(lang);
//            var languages = _menuService.GetMenuLanguages();
//            var menuVm = new MenuViewModel(menu, languages.ToList());
//            ViewBag.MainMenu = menuVm;
//        }
//    }
//}