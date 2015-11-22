﻿using System.Linq;
using System.Web.Mvc;
using UniversityWebsite.Model;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    public class HomeController : BaseController
    {
        public IPageService PageService { get; set; }

        public HomeController(IMenuService menuService, IPageService pageService)
            : base(menuService)
        {
            PageService = pageService;
        }

        public ActionResult Index()
        {
            var pages = PageService.GetParentlessPages((string)Session[Consts.SessionKeyLang]);
            var tileList = pages.Select(p => new TileViewModel { Title = p.Title, UrlName = p.UrlName }).ToList();
            return View(tileList);
        }
	}
}