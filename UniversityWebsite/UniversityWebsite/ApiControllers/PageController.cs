using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Helper;
using UniversityWebsite.Model.Page;
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
        //[AntiForgeryValidate]
        public IEnumerable<PageDto> GetPages()
        {
            return _pageService.GetAll();
        }
        [Route("{id:int}/availableLanguages")]
        public IEnumerable<Language> GetAvailableLanguages(int id)
        {
            var usedLanguages = _pageService.GetTranslationsLanguages(id);
            var allLanguages = _languageService.GetLanguagesCached();
            return allLanguages.Where(l => !usedLanguages.Contains(l.CountryCode));
        }

        // GET api/Page/5
        //[AntiForgeryValidate]
        [Route("{id:int}", Name = "GetPage")]
        [ResponseType(typeof(PageDto))]
        public IHttpActionResult GetPage(int id)
        {
            PageDto page = _pageService.FindPage(id);
            if (page == null)
                return NotFound();

            return Ok(page);
        }

        // PUT api/Page/5
        [Route("{name}")]
        //[AntiForgeryValidate]
        [ResponseType(typeof(PageDto))]
        public IHttpActionResult PutPage(PagePut page)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var updatedPage = _pageService.UpdatePage(Mapper.Map<PageDto>(page));
            return CreatedAtRoute("GetPage", new { id = updatedPage.Id }, updatedPage);
        }

        // POST api/Page
        [Route("")]
        //[AntiForgeryValidate]
        [ResponseType(typeof(PageDto))]
        public IHttpActionResult PostPage(PagePosted page)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var createdPage = _pageService.Add(Mapper.Map<PageDto>(page));
            return CreatedAtRoute("GetPage",new{ id = createdPage.Id}, createdPage);
        }

        // DELETE api/Page/5
        [Route("{name}")]
        //[AntiForgeryValidate]
        public IHttpActionResult DeletePage(int id)
        {
            _pageService.Delete(id);
            return Ok();
        }
    }
}