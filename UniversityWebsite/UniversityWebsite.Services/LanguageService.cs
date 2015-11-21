using System;
using System.Collections.Generic;
using System.Linq;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Helpers;

namespace UniversityWebsite.Services
{
    public interface ILanguageService
    {
        void AddLanguage(string countryCode);
        /// <summary>
        /// Trwale usuwa wszystkie encje z bazy istniejące w tym języku
        /// </summary>
        /// <param name="countryCode"></param>
        void DeleteLanguage(string countryCode);

        IEnumerable<Language> GetLanguagesCached();
    }
    public class LanguageService : ILanguageService
    {
        public LanguageService(IDomainContext context)
        {
            _context = context;
        }

        private IDomainContext _context;

        public void AddLanguage(string countryCode)
        {
            _context.Languages.Add(new Language{CountryCode = countryCode});
            _context.SaveChanges();
        }

        public void DeleteLanguage(string countryCode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Language> GetLanguagesCached()
        {
            List<Language> languages = CacheHelper.GetOrInvoke<List<Language>>(
                "Languages",
                () => _context.Languages.ToList(),
                TimeSpan.FromSeconds(10));//Todo
            return languages;
        }
    }
}
