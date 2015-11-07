using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UniversityWebsite.Core;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    [Authorize]
    public class UsersController : AccountController
    {
        public UsersController(IMenuService menuService, IPageService pageService) : base(menuService, pageService)
        {

        }

        public async Task<JsonResult> Find(string login)
        {
            return new JsonResult{Data = await UserManager.FindByNameAsync(login)};
        }
    }
}