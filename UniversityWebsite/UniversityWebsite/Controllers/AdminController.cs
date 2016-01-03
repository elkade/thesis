using System.Web.Mvc;

namespace UniversityWebsite.Controllers
{
    /// <summary>
    /// Odpowiada za zwrócenie widoku panelu administratora.
    /// </summary>
    [Authorize]
    public class AdminController : Controller
    {
        /// <summary>
        /// Zwraca widok panelu administratora.
        /// </summary>
        /// <returns>Obiekt widoku</returns>
        public ActionResult Index()
        {
            ViewBag.IsAdmin = User.IsInRole("Administrator");
            return View();
        }
    }
}