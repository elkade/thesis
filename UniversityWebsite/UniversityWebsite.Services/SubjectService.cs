using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    public interface ISubjectService
    {
        IEnumerable<Subject> GetSemester(int number);
        Subject GetSubject(string name);
        IEnumerable<SubjectDto> GetSubjects(int offset, int limit);
    }
    public class SubjectService : ISubjectService
    {
        private readonly IDomainContext _context;
        private const int SameTitleSubjectsMaxNumber = 100;

        public SubjectService(IDomainContext context)
        {
            _context = context;
        }

        public IEnumerable<Subject> GetSemester(int number)
        {
            return _context.Subjects.Where(s => s.Semester.Number == number);
            //if (number == 1)
            //    return new List<Subject>
            //    {
            //        new Subject {Name = "Elementy Logiki i Teorii Mnogości"},
            //        new Subject {Name = "Algebra"},
            //        new Subject {Name = "Analiza Matematyczna 1"}
            //    }; 
            //return new List<Subject>
            //{
            //    new Subject {Name = "Metody Translacji"},
            //    new Subject {Name = "Teoria Algorytmów i Języków"},
            //    new Subject {Name = "Seminarium Dyplomowe"}
            //};
        }

        public Subject GetSubject(string name)
        {
            var subject = _context.Subjects.Include(s=>s.Semester).SingleOrDefault(s => s.UrlName == name);
            return subject;
        }

        public IEnumerable<SubjectDto> GetSubjects(int offset, int limit)
        {
            if (limit < 0) return Enumerable.Empty<SubjectDto>();
            limit = limit > 50 ? 50 : limit;
            return
                _context.Subjects.OrderBy(s => s.Semester.Number)
                    .ThenBy(s => s.Name)
                    .Skip(offset)
                    .Take(limit)
                    .ProjectTo<SubjectDto>();
        }

        public Subject Add()
        {
            throw new NotImplementedException();
        }
        public string PrepareUniqueUrlName(string baseUrlName)
        {
            if (!_context.Subjects.Any(p => p.UrlName == baseUrlName))
                return baseUrlName;
            for (int i = 2; i < SameTitleSubjectsMaxNumber; i++)
            {
                string bufName = baseUrlName + i;
                if (!_context.Pages.Any(p => p.UrlName == bufName))
                    return bufName;
            }
            throw new PropertyValidationException("subject.UrlName", "Przekroczono liczbę przedmiotów o tym samym tytule.");
        }

    }
}
