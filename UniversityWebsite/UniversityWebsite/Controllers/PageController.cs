using System.Linq;
using System.Web.Mvc;
using UniversityWebsite.Domain;
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
            var context = new DomainContext();


            return View(context.Pages.FirstOrDefault());
        }
    }
}