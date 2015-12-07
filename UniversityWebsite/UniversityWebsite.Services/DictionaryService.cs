using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UniversityWebsite.Core;
using UniversityWebsite.Domain;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Helpers;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    //TODO scalić z language
    public interface IDictionaryService
    {
        /// <summary>
        /// Pobiera statyczny tekst z pamięci cache.
        /// Jeżeli tekst nie znajduje się w pamięci, wyszukuje w bazie danych.
        /// </summary>
        /// <param name="key">Klucz słowa</param>
        /// <param name="countryCode">Kod języka</param>
        /// <returns>Tekst</returns>
        string GetTranslationCached(string key, string countryCode);
        string GetTranslation(string key, string countryCode);
        IEnumerable<string> GetKeys();
        /// <summary>
        /// Pobiera listę kluczy słownika słów statycznych serwisu z pamięci cache.
        /// Jeżeli lista nie znajduje się w pamięci, wyszukuje w bazie danych.
        /// </summary>
        /// <returns>Wyliczenie kluczy</returns>
        IEnumerable<string> GetKeysCached();
        /// <summary>
        /// Wyszukuje słownik tekstów w danym języku.
        /// </summary>
        /// <param name="countryCode">Kod języka</param>
        /// <returns>Obiekt zawierający listę tekstów</returns>
        DictionaryDto GetDictionary(string countryCode);
    }
    public class DictionaryService : IDictionaryService
    {
        private readonly IDbSet<Phrase> _phrases;
        private IDomainContext _context;

        public DictionaryService(IDomainContext domainContext)
        {
            _phrases = domainContext.Phrases;
            _context = domainContext;
        }
        public string GetTranslationCached(string key, string countryCode)
        {
            string phrase = CacheHelper.GetOrInvoke<string>(
                string.Format("Phrase_{0}_{1}",key,countryCode),
                () => GetTranslation(key, countryCode),
                TimeSpan.FromSeconds(10));//Todo
            return phrase;
        }
        public string GetTranslation(string key, string countryCode)
        {
            var phrase = _phrases.SingleOrDefault(p => p.Key == key && p.CountryCode == countryCode);
            return phrase == null ? null : phrase.Value;
        }
        public IEnumerable<string> GetKeysCached()
        {
            List<string> keys = CacheHelper.GetOrInvoke<List<string>>(
                 "Keys",
                 ()=>GetKeys().ToList(),
                 TimeSpan.FromSeconds(10));//Todo
            return keys;

        }

        public DictionaryDto GetDictionary(string countryCode)
        {
            var lang = _context.Languages.SingleOrDefault(l => l.CountryCode == countryCode);
            if (lang == null)
                return null;
            var words = _context.Phrases.Where(p => p.CountryCode == countryCode)
                .ToList()
                .Select(p => new KeyValuePair<string, string>(p.Key, p.Value))
                .ToList();
            string title = lang.Title;
            return new DictionaryDto
            {
                Words = words,
                CountryCode = countryCode,
                Title = title
            };
        }

        public IEnumerable<string> GetKeys()
        {
            return _phrases.Select(p => p.Key).Distinct();
        }

    }
}
