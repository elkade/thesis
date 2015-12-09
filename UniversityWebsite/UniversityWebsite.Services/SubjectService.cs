﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Exceptions;

namespace UniversityWebsite.Services
{
    public interface ISubjectService
    {
        List<Subject> GetSemester(int number);
    }
    public class SubjectService : ISubjectService
    {
        private readonly IDomainContext _context;
        private const int SameTitleSubjectsMaxNumber = 100;

        public SubjectService(IDomainContext context)
        {
            _context = context;
        }

        public List<Subject> GetSemester(int number)
        {
            if (number == 1)
                return new List<Subject>
                {
                    new Subject {Name = "Elementy Logiki i Teorii Mnogości"},
                    new Subject {Name = "Algebra"},
                    new Subject {Name = "Analiza Matematyczna 1"}
                }; 
            return new List<Subject>
            {
                new Subject {Name = "Metody Translacji"},
                new Subject {Name = "Teoria Algorytmów i Języków"},
                new Subject {Name = "Seminarium Dyplomowe"}
            };
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