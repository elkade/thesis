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
    /// <summary>
    /// Kontroler api służący do wykonywania operacji CRUD na stronach systemu.
    /// </summary>
    [RoutePrefix("api/page")]
    public class PageController : ApiController
    {
        private readonly ILanguageService _languageService;
        private readonly IPageService _pageService;
        private readonly IMenuService _menuService;

        public PageController(IPageService pageService, ILanguageService languageService, IMenuService menuService)
        {
            _pageService = pageService;
            _languageService = languageService;
            _menuService = menuService;
        }
        // GET api/Page
        /// <summary>
        /// Zwraca wszystkie strony systemu.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        //[AntiForgeryValidate]
        public IEnumerable<PageDto> GetPages()
        {
            return _pageService.GetAll();
        }
        /// <summary>
        /// zwraca listę języków, na które może zostać przetłumaczona strona.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:int}/availableLanguages")]
        public IEnumerable<Language> GetAvailableLanguages(int id)
        {
            var usedLanguages = _pageService.GetTranslationsLanguages(id);
            var allLanguages = _languageService.GetLanguagesCached();
            return allLanguages.Where(l => !usedLanguages.Contains(l.CountryCode));
        }

        // GET api/Page/5
        //[AntiForgeryValidate]
        /// <summary>
        /// Zwraca stronę o danym id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Nadpisuje pola strony o danym id.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Dodaje nową stronę.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("")]
        //[AntiForgeryValidate]
        [ResponseType(typeof(PageDto))]
        public IHttpActionResult PostPage(PagePosted page)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var createdPage = _pageService.Add(Mapper.Map<PageDto>(page));
            if (page.IsTile)
                _menuService.AddToTilesMenuIfNotExists(createdPage.Id);
            return CreatedAtRoute("GetPage",new{ id = createdPage.Id}, createdPage);
        }

        // DELETE api/Page/5
        /// <summary>
        /// Usuwa stronę o danym id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:int}")]
        //[AntiForgeryValidate]
        public IHttpActionResult DeletePage(int id)
        {
            _pageService.Delete(id);
            return Ok();
        }
    }
}