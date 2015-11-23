using System.Threading.Tasks;
using System.Web.Mvc;
using UniversityWebsite.Core;
using UniversityWebsite.Services;

namespace UniversityWebsite.Controllers
{
    [Authorize]
    public class UsersController : AccountController
    {
        public async Task<JsonResult> Find(string login)
        {
            return new JsonResult{Data = await UserManager.FindByNameAsync(login)};
        }
    }
}