using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services;
using System.Collections.Generic;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.ApiControllers
{
    public class PageController : ApiController
    {
        private readonly ILanguageService _languageService;
        private readonly IPageService _pageService;

        public PageController(IPageService pageService, ILanguageService languageService)
        {
            _pageService = pageService;
            _languageService = languageService;
        }

        // GET api/Page
        public IEnumerable<PageDto> GetPages()
        {
            return _pageService.GetAll();
        }

        public IEnumerable<Language> GetAvailableLanguages(string name)
        {
            var usedLanguages = _pageService.GetTranslationsLanguages(name);
            var allLanguages = _languageService.GetLanguagesCached();
            return allLanguages.Where(l => !usedLanguages.Contains(l.CountryCode));
        }

        // GET api/Page/5
        //[ValidateCustomAntiForgeryToken]
        [ResponseType(typeof(Page))]
        public IHttpActionResult GetPage(string name)
        {
            PageDto page = _pageService.FindPage(name);
            if (page == null)
                return NotFound();

            return Ok(page);
        }

        // PUT api/Page/5
        public IHttpActionResult PutPage(string name, PageDto page)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (name != page.UrlName)
                return BadRequest();
            _pageService.UpdatePage(page);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Page
        [ResponseType(typeof(Page))]
        public IHttpActionResult PostPage(PageDto page)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            page.UrlName = HttpUtility.UrlEncode(page.Title);
            _pageService.Add(page);
            return CreatedAtRoute("DefaultApi", new { name = page.UrlName }, page);
        }

        // DELETE api/Page/5
        [ResponseType(typeof(Page))]
        public IHttpActionResult DeletePage(string name)
        {
            _pageService.Delete(name);
            return Ok();
        }
    }
}