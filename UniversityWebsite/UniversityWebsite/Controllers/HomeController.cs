using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using UniversityWebsite.Filters;
using UniversityWebsite.Model;
using UniversityWebsite.Services;

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
            var tiles = _menuService.GetTilesMenu((string)Session[Consts.SessionKeyLang]);
            return View(Mapper.Map<List<TileViewModel>>(tiles).ToList());
        }
	}
}