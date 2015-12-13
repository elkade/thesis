using System.Web.Http;
using AutoMapper;
using UniversityWebsite.Api.Model.Users;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services;

namespace UniversityWebsite.Api.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [Route("{userId}")]
        public IHttpActionResult GetUser(string userId)
        {
            var user = _userService.GetUser(userId);
            var userVm = Mapper.Map<UserVm>(user);
            return Ok(userVm);
        }
        [Route("")]
        public IHttpActionResult PostUser(UserVm user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            User dbUser = Mapper.Map<User>(user);
            var createdUser = _userService.CreateUser(dbUser, user.Role);
            return Ok(createdUser);
        }

    }
}