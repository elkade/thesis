using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
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

        [Route("")]
        [HttpGet]
        public IEnumerable<MenuDto> GetAll()
        {
            return _menuService.GetAll();
        }

        [Route("{lang}", Name = "GetMenu")]
        [HttpGet]
        [ResponseType(typeof(MenuDto))]
        public IHttpActionResult GetMenu(string lang)
        {
            return Ok(_menuService.GetMainMenuCached(lang));
        }
        [Route("{lang}")]
        [HttpPut]
        [ResponseType(typeof(MenuDto))]
        public IHttpActionResult PutMenu(MenuDto menu)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var updatedMenu = _menuService.UpdateMenu(Mapper.Map<MenuDto>(menu));
            return CreatedAtRoute("GetMenu", new { lang = updatedMenu.CountryCode }, updatedMenu);
        }
    }
}