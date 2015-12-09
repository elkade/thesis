using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UniversityWebsite.Model;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    public class TeachingController : Controller
    {
        private readonly ISubjectService _subjectService;

        public TeachingController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public ActionResult Index()
        {
            return View(new TeachingVm{SemestersCount = 10});
        }

        public ActionResult Semester(int number)
        {
            var subjects = _subjectService.GetSemester(number);
            if (subjects == null)
            {
                
            }
            return View(new SemesterVm{Subjects = subjects.Select(s=>new SubjectListElementVm{SubjectName = s.Name}).ToList()});
        }

    }
}