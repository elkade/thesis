using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using UniversityWebsite.Filters;
using UniversityWebsite.Model;
using UniversityWebsite.Services;
using UniversityWebsite.Model.Page;

namespace UniversityWebsite.Controllers
{
    /// <summary>
    /// Odpowiada za zwrócenie widoku strony głównej.
    /// </summary>
    [MainMenu]
    public class HomeController : Controller
    {
        private readonly IMenuService _menuService;
        private IPageService PageService { get; set; }

        /// <summary>
        /// Tworzy nową instancję kontrolera.
        /// </summary>
        /// <param name="menuService">Serwis odpowiedzialny za zarządzanie menu systemu</param>
        /// <param name="pageService">Serwis odpowiedzialny za zarządzanie stronami systemu</param>
        public HomeController(IMenuService menuService, IPageService pageService)
        {
            _menuService = menuService;
            PageService = pageService;
        }
        /// <summary>
        /// Zwraca widok strony głównej.
        /// </summary>
        /// <returns>Obiekt widoku</returns>
        public ActionResult Index()
        {
            var tiles = _menuService.GetTilesMenuCached((string)Session[Consts.SessionKeyLang]);
            var siblings = PageService.GetParentlessPagesWithChildren((string)Session[Consts.SessionKeyLang]).ToList();
            return View(new HomeVm { NavMenu = new NavMenuVm { IsTopLevel = true, Items = Mapper.Map<List<PageMenuItemVm>>(siblings) }, Tiles = Mapper.Map<List<TileViewModel>>(tiles).ToList() });
        }
	}
}