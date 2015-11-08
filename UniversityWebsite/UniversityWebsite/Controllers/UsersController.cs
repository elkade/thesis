using System.Threading.Tasks;
using System.Web.Mvc;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    [Authorize]
    public class UsersController : AccountController
    {
        public UsersController(IMenuService menuService, IPageService pageService, ILanguageService languageService) : base(menuService, pageService,languageService)
        {

        }

        public async Task<JsonResult> Find(string login)
        {
            return new JsonResult{Data = await UserManager.FindByNameAsync(login)};
        }
    }
}