using System.Collections.Generic;
using System.Web.Http;
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
        public IEnumerable<MenuDto> GetAll()
        {
            return _menuService.GetAll();
        }

        [Route("{lang}")]
        public MenuDto GetMenu(string lang)
        {
            return _menuService.GetMainMenuCached(lang);
        }
    }
}