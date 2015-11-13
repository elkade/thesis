using System.Linq;
using System.Web.Mvc;
using UniversityWebsite.Core;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IDomainContext _domainContext;

        public AdminController(IPageService pageService, IDomainContext domainContext)
        {
            _pageService = pageService;
            _domainContext = domainContext;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NavigationMenu()
        {
            return View();
        }
    }
}