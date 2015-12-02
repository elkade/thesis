using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using UniversityWebsite.Model.Menu;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.ApiControllers
{
    [RoutePrefix("api/menu")]
    //[AntiForgeryValidate]
    public class MenuController : ApiController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [Route("main")]
        [HttpGet]
        public IEnumerable<MenuDto> GetAllMain()
        {
            return _menuService.GetMainMenuGroup();
        }

        [Route("main/{lang}", Name = "GetMenu")]
        [HttpGet]
        [ResponseType(typeof(MenuDto))]
        public IHttpActionResult GetMenu(string lang)
        {
            return Ok(_menuService.GetMainMenuCached(lang));
        }
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
            menu.GroupId = 1;
            _menuService.UpdateMenuItems(menu);
            return Ok();
            //return CreatedAtRoute("GetMenu", new { lang = updatedMenu.CountryCode }, updatedMenu);
        }
    }
}