using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using UniversityWebsite.Model.Menu;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.ApiControllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za operacje pobierania menu głównego i edycję jego elementów.
    /// </summary>
    [RoutePrefix("api/menu")]
    
    //[AntiForgeryValidate]
    public class MenuController : ApiController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        /// <summary>
        /// Zwraca listę menu głównych systemu we wszystkich językach zdefiniowanych w systemie.
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
        [Route("main/{lang}", Name = "GetMainMenu")]
        [HttpGet]
        [ResponseType(typeof(MenuDto))]
        public IHttpActionResult GetMenu(string lang)
        {
            return Ok(_menuService.GetMainMenuCached(lang));
        }
        /// <summary>
        /// Nadpisuje pola menu głównego o podanym języku.
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
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