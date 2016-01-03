using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Model;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Controllers
{
    /// <summary>
    /// Odpowiada za składanie wniosków o zapisanie studenta na przedmiot
    /// </summary>
    public class SignUpController : Controller
    {
        private readonly ISubjectService _subjectService;

        /// <summary>
        /// Tworzy nową instancje kontrolera.
        /// </summary>
        /// <param name="subjectService">Serwis odpowiedzialny za zarządzanie przedmiotami systemu</param>
        public SignUpController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        /// <summary>
        /// Tworzy wniosek o zapisanie studenta na przedmiot.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <returns>Widok kontrolki informującej o statusie wniosku.</returns>
        [Authorize(Roles = "Student")]
        [HttpPost]
        public ActionResult SignUpForSubject(int subjectId)
        {
            var userId = User.Identity.GetUserId();
            _subjectService.SignUpForSubject(subjectId, userId);
            return View("~/Views/Teaching/_SignUpStatus.cshtml", new SubjectListElementVm{SubjectId = subjectId, SignUpAction = SignUpAction.Submitted});
        }

        /// <summary>
        /// Usuwa wniosek o zapisaie się na przdmiot.
        /// </summary>
        /// <param name="subjectId">Id przedmiotu</param>
        /// <returns>Widok kontrolki informującej o statusie wniosku.</returns>
        public ActionResult ResignFromSubject(int subjectId)
        {
            var userId = User.Identity.GetUserId();
            _subjectService.ResignFromSubject(subjectId, userId);
            return View("~/Views/Teaching/_SignUpStatus.cshtml", new SubjectListElementVm { SubjectId = subjectId, SignUpAction = SignUpAction.NotSubmitted });
        }
    }
}