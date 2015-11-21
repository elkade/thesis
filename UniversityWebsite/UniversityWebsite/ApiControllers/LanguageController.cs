using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services;

namespace UniversityWebsite.ApiControllers
{
    public class LanguageController : ApiController
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        // GET api/Language
        public IEnumerable<Language> GetLanguages()
        {
            return _languageService.GetLanguagesCached();
        }

        // POST api/Page
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
