using System.Linq;
using System.Web.Mvc;
using UniversityWebsite.Core;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    [Authorize]
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

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View(_domainContext.Users.ToList());
        }

        public ActionResult Pages()
        {
            return View(_pageService.GetAll());
        }

        public ActionResult Subjects()
        {
            return View();
        }

        public ActionResult NavigationMenu()
        {
            return View();
        }
    }
}