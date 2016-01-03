using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Api.Controllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za operacje pobierania menu głównego i edycję jego elementów.
    /// </summary>
    [RoutePrefix("api/menu")]
    
    //[AntiForgeryValidate]
    public class MenuController : ApiController
    {
        private readonly IMenuService _menuService;

        /// <summary>
        /// Tworzy nową instancję kontrolera
        /// </summary>
        /// <param name="menuService">Serwis zarządzający menu zawartymi w systemie</param>
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        /// <summary>
        /// Zwraca listę menu głównych systemu we wszystkich językach zdefiniowanych w systemie.
        /// </summary>
        /// <returns>Lista menu</returns>
        [Route("main")]
        [HttpGet]
        public IEnumerable<MenuDto> GetAllMain()
        {
            return _menuService.GetMenuGroup(_menuService.MainMenuGroupId);
        }
        /// <summary>
        /// Zwraca menu główne systemu w podanym języku.
        /// </summary>
        /// <param name="lang"></param>
        /// <returns>Menu</returns>
        [Route("main/{lang}", Name = "GetMainMenu")]
        [HttpGet]
        [ResponseType(typeof(MenuDto))]
        public IHttpActionResult GetMenu(string lang)
        {
            return Ok(_menuService.GetMainMenu(lang));
        }
        /// <summary>
        /// Nadpisuje pola menu głównego o podanym języku.
        /// </summary>
        /// <param name="lang">Kod danego języka</param>
        /// <param name="menu">Dane menu, którymi ma zostać nadpisane istniejące menu</param>
        /// <returns>Dane menu po nadpisaniu</returns>
        [Route("main/{lang}")]
        [HttpPut]
        [HttpPost]
        //[ResponseType(typeof(MenuDto))]
        public IHttpActionResult UpdateMainMenu(string lang, MenuData menu)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (lang != menu.CountryCode)
                return BadRequest("Language mismatch");
            menu.GroupId = _menuService.MainMenuGroupId;
            _menuService.UpdateMenuItems(menu);
            return Ok();
            //return CreatedAtRoute("GetMenu", new { lang = updatedMenu.CountryCode }, updatedMenu);
        }
    }
}