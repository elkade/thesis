using System.Linq;
using System.Web.Mvc;
using UniversityWebsite.Domain;
using UniversityWebsite.Services;
using UniversityWebsite.ViewModels;

namespace UniversityWebsite.Controllers
{
    public class PageController : Controller
    {

        [HttpGet]
        public ActionResult Index(string pageName)
        {
            var service = new PageService();

            return View(service.FindPage(pageName));
        }
    }
}