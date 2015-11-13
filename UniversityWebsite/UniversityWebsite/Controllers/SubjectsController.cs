using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.ViewModels;

namespace UniversityWebsite.Controllers
{
    public class SubjectsController : ApiController
    {
        private readonly IDomainContext _context;
        public SubjectsController()
        {
            _context = new DomainContext();
        }

        public IEnumerable<SubjectViewModel> GetAllSubjects()
        {
            return _context.Subjects.Select(Mapper.Map<SubjectViewModel>);
        }

        [HttpGet]
        public SubjectViewModel GetPage(string subjectName)
        {
            var subject = _context.Subjects.FirstOrDefault(s => s.Name == subjectName);
            return new SubjectViewModel{Name = subject.Name};
        }

        [HttpPost]
        public void Put(Subject subject)
        {
            _context.Subjects.Add(subject);
            _context.SaveChanges();
        }
         
    }
}