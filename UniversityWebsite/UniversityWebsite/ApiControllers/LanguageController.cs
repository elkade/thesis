using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services;

namespace UniversityWebsite.ApiControllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za operacje CRUD na językach dostępnych w systemie.
    /// </summary>
    public class LanguageController : ApiController
    {
        private readonly ILanguageService _languageService;
        /// <summary>
        /// Konstruktor przyjmujący serwis do obsługi języków.
        /// </summary>
        /// <param name="languageService"></param>
        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        // GET api/Language
        /// <summary>
        /// Zwraca listę języków zdefiniowanych w systemie.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Language> GetLanguages()
        {
            return _languageService.GetLanguagesCached();
        }

        // POST api/Page
        /// <summary>
        /// Dodaje nowy język.
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        [ResponseType(typeof(string))]
        public IHttpActionResult AddLanguage(string countryCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _languageService.AddLanguage(countryCode);

            return Ok(countryCode);
        }

    }
}
