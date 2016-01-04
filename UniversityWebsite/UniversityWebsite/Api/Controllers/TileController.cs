using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Api.Controllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za operacje pobierania menu kafelków i edycję jego elementów.
    /// </summary>
    [RoutePrefix("api/tile")]
    [Authorize(Roles = Consts.AdministratorRole)]
    //[AntiForgeryValidate]
    public class TileController : ApiController
    {
        private readonly IMenuService _menuService;
        /// <summary>
        /// Konstruktor przyjmujący serwis menu.
        /// </summary>
        /// <param name="menuService"></param>
        public TileController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        /// <summary>
        /// Zwraca listę menu głównych w różnych językach
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public IEnumerable<MenuDto> GetAllMain()
        {
            return _menuService.GetMenuGroup(_menuService.TilesMenuGroupId);
        }
        /// <summary>
        /// Zwraca menu główne w danym języku.
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        [Route("{lang}", Name = "GetTilesMenu")]
        [HttpGet]
        [ResponseType(typeof (List<Tile>))]
        public IHttpActionResult GetMenu(string lang)
        {
            return Ok(_menuService.GetTilesMenu(lang));
        }
        /// <summary>
        /// Nadpisuje elementy menu o danym języku podanymi w body zapytania. 
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        [Route("{lang}")]
        [HttpPut]
        [HttpPost]
        //[ResponseType(typeof(MenuDto))]
        public IHttpActionResult UpdateTilesMenu(string lang, MenuData menu)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (lang != menu.CountryCode)
                return BadRequest("Language mismatch");
            menu.GroupId = _menuService.TilesMenuGroupId;
            _menuService.UpdateMenuItems(menu);
            return Ok();
            //return CreatedAtRoute("GetMenu", new { lang = updatedMenu.CountryCode }, updatedMenu);
        }
    }
}