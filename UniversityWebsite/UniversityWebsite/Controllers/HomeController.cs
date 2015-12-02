using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using UniversityWebsite.Filters;
using UniversityWebsite.Model;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
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

        public ActionResult Index()
        {
            var tiles = _menuService.GetTilesMenu((string)Session[Consts.SessionKeyLang]);
            return View(tiles.Select(t=>new TileViewModel{Title = t.Title, UrlName = t.UrlName}).ToList());
        }
	}
}