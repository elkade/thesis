using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using UniversityWebsite.Filters;
using UniversityWebsite.Model;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Model.Page;
using System.Collections.Generic;

namespace UniversityWebsite.Controllers
{
    [MainMenu]
    public class TeachingController : Controller
    {
        private readonly ISubjectService _subjectService;
        private readonly IPageService _pageService;

        public TeachingController(ISubjectService subjectService, IPageService pageService)
        {
            _subjectService = subjectService;
            _pageService = pageService;
        }

        public ActionResult Index()
        {
            var siblings = _pageService.GetParentlessPagesWithChildren((string)Session[Consts.SessionKeyLang]).ToList();
            return View(new TeachingVm { SemestersCount = 10, Siblings = Mapper.Map<List<PageMenuItemVm>>(siblings) });
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