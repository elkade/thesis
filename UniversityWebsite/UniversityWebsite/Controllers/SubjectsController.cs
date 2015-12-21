using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [Authorize(Roles = "Student")]
        public ActionResult SignUp(int subjectId)
        {
            var userId = User.Identity.GetUserId();
            _subjectService.SignUpForSubject(subjectId, userId);
            return null;
        }

    }
}