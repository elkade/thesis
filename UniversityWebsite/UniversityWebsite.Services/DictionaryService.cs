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
    public interface IDictionaryService
    {
        string GetTranslationCached(string key, string countryCode);
        string GetTranslation(string key, string countryCode);
        IEnumerable<string> GetKeys();
        IList<string> GetKeysCached();

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
        public IList<string> GetKeysCached()
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
