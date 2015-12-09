using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Model;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.ApiControllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za operacje CRUD na językach dostępnych w systemie.
    /// </summary>
    [RoutePrefix("api/languages")]
    public class LanguageController : ApiController
    {
        private readonly ILanguageService _languageService;
        private readonly IDictionaryService _dictionaryService;

        /// <summary>
        /// Konstruktor przyjmujący serwis do obsługi języków.
        /// </summary>
        /// <param name="languageService"></param>
        /// <param name="dictionaryService"></param>
        public LanguageController(ILanguageService languageService, IDictionaryService dictionaryService)
        {
            _languageService = languageService;
            _dictionaryService = dictionaryService;
        }

        // GET api/Language
        /// <summary>
        /// Zwraca listę języków zdefiniowanych w systemie.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IEnumerable<Language> GetLanguages()
        {
            return _languageService.GetLanguagesCached();
        }

        // POST api/Page
        /// <summary>
        /// Dodaje nowy język.
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="newLanguage"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{lang}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult AddLanguage(string lang, [FromBody]NewLanguage newLanguage )
        {
            if (newLanguage.CountryCode != lang)
                return BadRequest("countryCode values int url and body are not the same.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _languageService.AddLanguage(Mapper.Map<DictionaryDto>(newLanguage));

            return Ok(lang);
        }
        /// <summary>
        /// Zwraca listę kluczy słownika statycznych słów systemu.
        /// </summary>
        /// <returns></returns>
        [Route("keys")]
        [HttpGet]
        public IEnumerable<string> GetKeys()
        {
            var keys = _dictionaryService.GetKeysCached();

            return keys;
        }
        /// <summary>
        /// Zwraca listę kluczy słownika statycznych słów systemu.
        /// </summary>
        /// <returns></returns>
        [Route("{lang}/dictionary")]
        [HttpGet]
        [ResponseType(typeof(DictionaryDto))]
        public IHttpActionResult GetDictionary(string lang)
        {
            var dictionary = _dictionaryService.GetDictionary(lang);

            if (dictionary == null)
                return NotFound();

            return Ok(dictionary);
        }

        /// <summary>
        /// Zwraca listę słowników w każdym języku systemu.
        /// </summary>
        /// <returns></returns>
        [Route("dictionaries")]
        [HttpGet]
        public IEnumerable<DictionaryDto> GetDictionaries()
        {
            var dictionaries = _dictionaryService.GetDictionaries();

            return dictionaries;
        }

        /// <summary>
        /// Aktualizuje listę słowników w każdym języku systemu.
        /// </summary>
        /// <returns>Lista słowników</returns>
        [Route("dictionaries")]
        [HttpPut]
        public IEnumerable<DictionaryDto> UpdateDictionaries(List<DictionaryDto> dictionaries )
        {
            var dictionariesUpdated = _dictionaryService.UpdateDictionaries(dictionaries);

            return dictionariesUpdated;
        }
    }
}
