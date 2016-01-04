using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using UniversityWebsite.Api.Model;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Filters;
using UniversityWebsite.Helper;
using UniversityWebsite.Model.Page;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Api.Controllers
{
    /// <summary>
    /// Kontroler api służący do wykonywania operacji CRUD na stronach systemu.
    /// </summary>
    [RoutePrefix("api/page")]
    [Authorize(Roles=Consts.AdministratorRole)]
    public class PageController : ApiController
    {
        private readonly ILanguageService _languageService;
        private readonly IPageService _pageService;
        private readonly IMenuService _menuService;

        /// <summary>
        /// Tworzy nową instancje kontrolera.
        /// </summary>
        /// <param name="pageService">Serwis zarządzający stronami dostępnymi w systemie</param>
        /// <param name="languageService">Serwis zarządzający językami dostępnymi w systemie</param>
        /// <param name="menuService">Serwis zarządzający menu zawartymi w systemie</param>
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
        /// <param name="limit">Maksymalna liczba zwrócowych obiektów</param>
        /// <param name="offset">Numer porządkowy pierwszego obiektu listy</param>
        /// <returns>Zbiór stron</returns>
        [Route("")]
        [Limit(50), Offset]
        //[AntiForgeryValidate]
        public IHttpActionResult GetPages(int limit = 50, int offset = 0)
        {
            var pages = _pageService.GetAll(limit, offset);
            var number = _pageService.GetPagesNumber();
            return Ok(new PaginationVm<PageDto>(pages, number, limit, offset));
        }

        /// <summary>
        /// Zwraca wszystkie strony systemu w danym języku
        /// </summary>
        /// <param name="lang">język, w którym mają zostać wyszukane strony</param>
        /// <param name="limit">Maksymalna liczba zwrócowych obiektów</param>
        /// <param name="offset">Numer porządkowy pierwszego obiektu listy</param>
        /// <returns>Zbiór stron</returns>
        [Route("")]
        [Limit(50), Offset]
        //[AntiForgeryValidate]
        public PaginationVm<PageDto> GetPagesByLang(string lang, int limit = 50, int offset = 0)
        {
            var pages = _pageService.GetPagesByCountryCode(lang, limit, offset);
            var number = _pageService.GetPagesNumberByCountryCode(lang);
            return new PaginationVm<PageDto>(pages, number, limit, offset);
        }
        /// <summary>
        /// zwraca listę języków, na które może zostać przetłumaczona strona.
        /// </summary>
        /// <param name="id">Id danej strony</param>
        /// <returns>Zbiór języków</returns>
        [Route("{id:int}/availableLanguages")]
        public IEnumerable<Language> GetAvailableLanguages(int id)
        {
            var usedLanguages = _pageService.GetTranslationsLanguages(id);
            var allLanguages = _languageService.GetLanguagesCached();
            return allLanguages.Where(l => !usedLanguages.Contains(l.CountryCode));
        }

        
        /// <summary>
        /// Zwraca stronę o danym id.
        /// </summary>
        /// <param name="id">Id strony</param>
        /// <returns>Dane strony o podanym id</returns>
        [Route("{id:int}", Name = "GetPage")]
        [ResponseType(typeof(PageDto))]
        //[AntiForgeryValidate]
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
        /// <param name="page">Dane, którymi ma zostać nadpisana strona</param>
        /// <returns>Dane strony po aktualizacji</returns>
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
        /// <param name="page">Dane nowej strony</param>
        /// <returns>Dane nowej strony po dodaniu</returns>
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
        /// <param name="id">Id strony do usunięcia</param>
        /// <returns>Status HTTP</returns>
        [Route("{id:int}")]
        //[AntiForgeryValidate]
        public IHttpActionResult DeletePage(int id)
        {
            _pageService.Delete(id);
            return Ok();
        }
    }
}