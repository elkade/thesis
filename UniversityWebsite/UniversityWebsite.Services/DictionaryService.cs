﻿using System.Data.Entity;
using System.Linq;
using UniversityWebsite.Core;
using UniversityWebsite.Domain;

namespace UniversityWebsite.Services
{
    public interface IDictionaryService
    {
        string GetTranslation(string id, string lang);
    }
    public class DictionaryService : IDictionaryService
    {
        protected IDbSet<Phrase> Phrases;

        public DictionaryService(IDomainContext domainContext)
        {
            Phrases = domainContext.Phrases;
        }
        public string GetTranslation(string id, string lang)
        {
            var phrase = Phrases.SingleOrDefault(p=>p.Group == id && p.Language.CountryCode == lang);
            return phrase == null ? null : phrase.Text;
        }
    }
}