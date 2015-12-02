using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.ApiControllers
{
    [RoutePrefix("api/tile")]
    //[AntiForgeryValidate]
    public class TileController : ApiController
    {
        private readonly IMenuService _menuService;

        public TileController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<MenuDto> GetAllMain()
        {
            return _menuService.GetMenuGroup(_menuService.TilesMenuGroupId);
        }

        [Route("{lang}", Name = "GetTilesMenu")]
        [HttpGet]
        [ResponseType(typeof (List<Tile>))]
        public IHttpActionResult GetMenu(string lang)
        {
            return Ok(_menuService.GetTilesMenu(lang));
        }

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