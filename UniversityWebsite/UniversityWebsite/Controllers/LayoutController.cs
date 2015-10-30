using System.Web.Mvc;
using UniversityWebsite.Services;
using UniversityWebsite.ViewModels;

namespace UniversityWebsite.Controllers
{
    public class LayoutController : Controller
    {
        private IMenuService _menuService;

        public LayoutController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public ActionResult MainMenu()
        {
            var mainMenuData = _menuService.GetMainMenu(1);
            MenuViewModel menu = new MenuViewModel(mainMenuData);
            return PartialView("_MainMenu",menu);
        }
	}
}