using System.Web.Mvc;
using UniversityWebsite.Services;
using UniversityWebsite.ViewModels;

namespace UniversityWebsite.Controllers
{
    public class BaseController : Controller
    {
        private readonly IMenuService _menuService;
        private int _langId;

        public BaseController(IMenuService menuService)
        {
            _menuService = menuService;
            _langId = 1;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var mainMenuData = _menuService.GetMainMenu(_langId);
            MenuViewModel menu = new MenuViewModel(mainMenuData);
            ViewData["Menu"] = menu;
        }
    }
}