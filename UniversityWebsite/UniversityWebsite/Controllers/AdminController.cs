using System.Web.Mvc;
using UniversityWebsite.Core;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    /// <summary>
    /// Odpowiada za zwrócenie widoku panelu administratora.
    /// </summary>
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

        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }
    }
}