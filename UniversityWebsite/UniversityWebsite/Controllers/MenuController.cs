using System.Web.Mvc;
using AutoMapper;
using UniversityWebsite.Model.Menu;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public ActionResult Index()
        {
            string lang = (string)Session[Consts.SessionKeyLang];

            var menu = _menuService.GetMainMenuCached(lang);
            var menuVm = Mapper.Map<MenuViewModel>(menu);
            return View("_MainMenu", menuVm);
        }
    }
}