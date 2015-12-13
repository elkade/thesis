using System.Web.Http;
using UniversityWebsite.Services;

namespace UniversityWebsite.Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
    }
}