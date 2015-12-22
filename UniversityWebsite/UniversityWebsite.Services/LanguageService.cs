using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UniversityWebsite.Core;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Helpers;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    /// <summary>
    /// Serwis realizujący logikę biznesową dotyczącą języków systemu.
    /// </summary>
    public interface ILanguageService
    {
        /// <summary>
        /// Dodaje do bazy nowy język, pełny słownik tłumaczeń statycznych fraz systemu i nowe puste menu główne i kafelków.
        /// </summary>
        /// <param name="newLanguage">Obiekt zawierający dane nowego menu</param>
        void AddLanguage(DictionaryDto newLanguage);
        /// <summary>
        /// Trwale usuwa wszystkie encje z bazy istniejące w tym języku
        /// </summary>
        /// <param name="countryCode"></param>
        void DeleteLanguage(string countryCode);
        /// <summary>
        /// Sprawdza, czy dany język istnieje w bazie.
        /// </summary>
        /// <param name="countryCode">Kod języka</param>
        /// <returns>Wartość logiczna</returns>
        bool Exists(string countryCode);
        /// <summary>
        /// Pobiera listę języków z pamięci cache.
        /// Jeżeli lista nie znajduje się w pamięci, wyszukuje w bazie danych.
        /// </summary>
        /// <returns>Wyliczenie języków</returns>
        IEnumerable<Language> GetLanguagesCached();
    }
    /// <summary>
    /// Implementacja serwisu realizującego logikę biznesową dotyczącą języków systemu.
    /// </summary>
    public class LanguageService : ILanguageService
    {
        /// <summary>
        /// Tworzy nową instancję serwisu.
        /// </summary>
        /// <param name="context"></param>
        public LanguageService(IDomainContext context)
        {
            _context = context;
        }

        private IDomainContext _context;

        public void AddLanguage(DictionaryDto newLanguage)
        {
            var keys = _context.Phrases.Select(p => p.Key).Distinct().ToList();
            if (!keys.SequenceEqual(newLanguage.Words.Select(w => w.Key)))
                throw new PropertyValidationException("newLanguage.words", "Keys are not same as defined in the system.");
            if(_context.Languages.Any(l=>l.CountryCode==newLanguage.CountryCode))
                throw new PropertyValidationException("newLanguage.countryCode", "Language with specified countryCode already exists.");
            //if (_context.Languages.Any(l => l.Title == newLanguage.Title))
            //    throw new PropertyValidationException("newLanguage.title", "Language with specified name already exists.");
            var language = _context.Languages.Add(new Language { CountryCode = newLanguage.CountryCode, Title = newLanguage.Title});
            foreach (var word in newLanguage.Words)
            {
                _context.Phrases.Add(new Phrase
                {
                    CountryCode = newLanguage.CountryCode,
                    Key = word.Key,
                    Value = word.Value
                });
            }

            var mainMenuGroup = _context.Menus.Include(m=>m.Group).First(m => m.GroupId == 1).Group;
            var tilesMenuGroup = _context.Menus.Include(m => m.Group).First(m => m.GroupId == 2).Group;

            _context.Menus.Add(new Menu { Language = language, Group = mainMenuGroup });
            _context.Menus.Add(new Menu { Language = language, Group = tilesMenuGroup });

            _context.SaveChanges();
        }

        public void DeleteLanguage(string countryCode)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string countryCode)
        {
            return GetLanguagesCached().Select(l => l.CountryCode).Contains(countryCode);
        }

        public IEnumerable<Language> GetLanguagesCached()
        {
            var languages = CacheHelper.GetOrInvoke<List<Language>>(
                "Languages",
                () => _context.Languages.ToList(),
                TimeSpan.FromSeconds(10));//Todo
            return languages;
        }
    }
}
