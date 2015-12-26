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
        public IPageService PageService { get; set; }

        public HomeController(IMenuService menuService, IPageService pageService)
        {
            _menuService = menuService;
            PageService = pageService;
        }
        /// <summary>
        /// Zwraca widok strony głównej.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var tiles = _menuService.GetTilesMenuCached((string)Session[Consts.SessionKeyLang]);
            var siblings = PageService.GetParentlessPagesWithChildren((string)Session[Consts.SessionKeyLang]).ToList();
            return View(new HomeVm { Siblings = Mapper.Map<List<PageMenuItemVm>>(siblings), Tiles = Mapper.Map<List<TileViewModel>>(tiles).ToList() });
        }
	}
}