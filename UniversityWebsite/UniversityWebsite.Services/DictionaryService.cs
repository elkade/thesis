using System;
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
    }
    public class DictionaryService : IDictionaryService
    {
        private readonly IDbSet<Phrase> _phrases;

        public DictionaryService(IDomainContext domainContext)
        {
            _phrases = domainContext.Phrases;
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
    }
}
