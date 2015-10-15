using System.Linq;
using System.Web.Mvc;
using UniversityWebsite.Domain;
using UniversityWebsite.Services;
using UniversityWebsite.ViewModels;

namespace UniversityWebsite.Controllers
{
    public class TeachingController : Controller
    {
        public TeachingController()
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
    }
}