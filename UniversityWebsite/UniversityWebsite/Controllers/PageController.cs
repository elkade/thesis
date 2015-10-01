using System.Linq;
using System.Web.Mvc;
using UniversityWebsite.Services;
using UniversityWebsite.ViewModels;

namespace UniversityWebsite.Controllers
{
    public class PageController : Controller
    {
        private readonly ITilesService _tilesService;

        public PageController(ITilesService tilesService)
        {
            _tilesService = tilesService;
        }

        public ActionResult Index()
        {
            var tiles = _tilesService.GetTiles().Select(t => new TileVm
            {
                Date = t.Date,
                Href = t.Href,
                Header = t.Header,
                Paragraph = t.Paragraph
            })
                .ToList();
            return View(tiles);
        }
    }
}