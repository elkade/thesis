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
        /// <summary>
        /// Pobiera statyczny tekst z bazy danych.
        /// </summary>
        /// <param name="key">Klucz słowa</param>
        /// <param name="countryCode">Kod języka</param>
        /// <returns>Tekst</returns>
        string GetTranslation(string key, string countryCode);
        /// <summary>
        /// Pobiera listę kluczy słownika słów statycznych serwisu z bazy danych
        /// </summary>
        /// <returns>Wyliczenie kluczy</returns>
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
        /// <summary>
        /// Zwraca listę słowników we wszystkich językach systemu
        /// </summary>
        /// <returns></returns>
        IEnumerable<DictionaryDto> GetDictionaries();
        /// <summary>
        /// Aktualizuje słowa wszystkich słowników.
        /// </summary>
        /// <param name="dictionaries"></param>
        /// <returns>Lista par klucz-wartość które zostały zaktualizowane pogrupowana w słowniki</returns>
        IEnumerable<DictionaryDto> UpdateDictionaries(List<DictionaryDto> dictionaries);
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
                string.Format("Phrase_{0}_{1}", key, countryCode),
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
                 () => GetKeys().ToList(),
                 TimeSpan.FromSeconds(10));//Todo
            return keys;

        }

        public DictionaryDto GetDictionary(string countryCode)
        {
            var lang = _context.Languages.SingleOrDefault(l => l.CountryCode == countryCode);
            if (lang == null)
                return null;
            var words = _context.Phrases.Where(p => p.CountryCode == countryCode)
                .ToList().ToDictionary(p => p.Key, p => p.Value);
            string title = lang.Title;
            return new DictionaryDto
            {
                Words = words,
                CountryCode = countryCode,
            };
        }

        public IEnumerable<DictionaryDto> GetDictionaries()
        {
            var dictionaries = _context.Phrases.ToList()
                .GroupBy(p => p.CountryCode,
                               p => p,
                               (key, g) => new DictionaryDto
                               {
                                   CountryCode = key,
                                   Words = g.ToDictionary(p => p.Key, p => p.Value)
                               }
                              );
            return dictionaries;
        }

        public IEnumerable<DictionaryDto> UpdateDictionaries(List<DictionaryDto> dictionaries)
        {
            foreach (var dict in dictionaries)
            {
                var words = UpdateDictionary(dict);
                yield return new DictionaryDto { CountryCode = dict.CountryCode, Words = words.ToDictionary(p => p.Key, p => p.Value) };
            }
            _context.SaveChanges();
        }

        private IEnumerable<Phrase> UpdateDictionary(DictionaryDto dict)
        {
            foreach (var row in dict.Words)
            {
                var phrase = _context.Phrases.SingleOrDefault(p => p.CountryCode == dict.CountryCode && p.Key == row.Key);
                if (phrase == null)
                    throw new NotFoundException("klucz "+row.Key +" w języku "+dict.CountryCode);
                if(phrase.Value == row.Value) continue;
                phrase.Value = row.Value;
                _context.Entry(phrase).State = EntityState.Modified;
                yield return phrase;
            }
        }

        public IEnumerable<string> GetKeys()
        {
            return _phrases.Select(p => p.Key).Distinct();
        }

    }
}
