using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using UniversityWebsite.Filters;
using UniversityWebsite.Model;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Exceptions;

namespace UniversityWebsite.Controllers
{
    [MainMenu]
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
            return View(new SemesterVm{Subjects = subjects.Select(s=>new SubjectListElementVm{SubjectName = s.Name, SubjectUrlName = s.UrlName}).ToList()});
        }

        public ActionResult Subject(string name)
        {
            var subject = _subjectService.GetSubject(name);
            if(subject==null)
                throw new NotFoundException("subject");
            var subjectVm = Mapper.Map<SubjectVm>(subject);
            return View(subjectVm);
        }
    }
}