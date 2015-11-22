using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Helper;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.ApiControllers
{
    [RoutePrefix("api/page")]
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
        [Route("")]
        [AntiForgeryValidate]
        public IEnumerable<PageDto> GetPages()
        {
            return _pageService.GetAll();
        }
        [Route("{name}/availableLanguages")]
        public IEnumerable<Language> GetAvailableLanguages(string name)
        {
            var usedLanguages = _pageService.GetTranslationsLanguages(name);
            var allLanguages = _languageService.GetLanguagesCached();
            return allLanguages.Where(l => !usedLanguages.Contains(l.CountryCode));
        }

        // GET api/Page/5
        [AntiForgeryValidate]
        [Route("{name}", Name = "GetPage")]
        [ResponseType(typeof(PageDto))]
        public IHttpActionResult GetPage(string name)
        {
            PageDto page = _pageService.FindPage(name);
            if (page == null)
                return NotFound();

            return Ok(page);
        }

        // PUT api/Page/5
        [Route("{name}")]
        [AntiForgeryValidate]
        public IHttpActionResult PutPage(PageDto page)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            page.UrlName = HttpUtility.UrlEncode(page.Title);
            _pageService.UpdatePage(page);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Page
        [Route("")]
        [AntiForgeryValidate]
        [ResponseType(typeof(PageDto))]
        public IHttpActionResult PostPage(PageDto page)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            page.UrlName = HttpUtility.UrlEncode(page.Title);
            var createdPage = _pageService.Add(page);
            return CreatedAtRoute("GetPage",new{ name = createdPage.UrlName}, createdPage);
        }

        // DELETE api/Page/5
        [Route("{name}")]
        [AntiForgeryValidate]
        public IHttpActionResult DeletePage(string name)
        {
            _pageService.Delete(name);
            return Ok();
        }
    }
}