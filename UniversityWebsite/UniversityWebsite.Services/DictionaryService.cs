using System.Data.Entity;
using System.Linq;
using UniversityWebsite.Core;
using UniversityWebsite.Domain;
using UniversityWebsite.Domain.Model;

namespace UniversityWebsite.Services
{
    public interface IDictionaryService
    {
        string GetTranslation(string key, string countryCode);
    }
    public class DictionaryService : IDictionaryService
    {
        protected IDbSet<Phrase> Phrases;

        public DictionaryService(IDomainContext domainContext)
        {
            Phrases = domainContext.Phrases;
        }
        public string GetTranslation(string key, string countryCode)
        {
            var phrase = Phrases.SingleOrDefault(p => p.Key == key && p.CountryCode == countryCode);
            return phrase == null ? null : phrase.Value;
        }
    }
}
