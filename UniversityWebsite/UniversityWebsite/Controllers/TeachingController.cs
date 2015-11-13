using System.Web.Mvc;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    public class TeachingController : BaseController
    {
        public TeachingController(IMenuService menuService, IPageService pageService, ILanguageService languageService) : base(menuService, pageService, languageService)
        {
        }

        [HttpGet]
        public Subject GetSubject(string subjectName)
        {
            return new Subject
            {
                Name = "dasdasd"
            };
        }

        [HttpGet]
        public ActionResult SubjectTemp()
        {
            return View();
        }
    }
}