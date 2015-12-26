using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Model;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Controllers
{
    public class SignUpController : Controller
    {
        private readonly ISubjectService _subjectService;

        public SignUpController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public ActionResult SignUpForSubject(int subjectId)
        {
            var userId = User.Identity.GetUserId();
            _subjectService.SignUpForSubject(subjectId, userId);
            return View("~/Views/Teaching/_SignUpStatus.cshtml", new SubjectListElementVm{SubjectId = subjectId, SignUpAction = SignUpAction.Submitted});
        }

        public ActionResult ResignFromSubject(int subjectId)
        {
            var userId = User.Identity.GetUserId();
            _subjectService.ResignFromSubject(subjectId, userId);
            return View("~/Views/Teaching/_SignUpStatus.cshtml", new SubjectListElementVm { SubjectId = subjectId, SignUpAction = SignUpAction.NotSubmitted });
        }
    }
}