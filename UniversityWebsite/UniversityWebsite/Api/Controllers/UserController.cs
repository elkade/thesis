using System.Collections.Generic;
using System.Web.Http;
using UniversityWebsite.Api.Model;
using UniversityWebsite.Api.Model.Users;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Filters;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Api.Controllers
{
    [RoutePrefix("api/users")]
    [Authorize(Roles = Consts.AdministratorRole)]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("")]
        [Limit(50), Offset]
        public PaginationVm<UserDto> GetUsers(string role = null, int limit = 50, int offset = 0)
        {
            IEnumerable<UserDto> users = role == null ?
                _userService.GetUsers(limit, offset) :
                _userService.GetUsersByRole(role, limit, offset);

            int number = role == null ?
                _userService.GetUsersNumber() :
                _userService.GetUsersNumberByRole(role);

            return new PaginationVm<UserDto>(users, number, limit, offset);
        }

        [Route("{userId:guid}")]
        public IHttpActionResult GetUser(string userId)
        {
            var user = _userService.GetUser(userId);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
        [Route("")]
        public IHttpActionResult PostUser(PostUserVm model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userDto = new UserDto
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IndexNumber = model.IndexNumber,
                Pesel = model.Pesel,
                Role = model.Role
            };
            try
            {
                var user = _userService.CreateUser(userDto);
                return Ok(user);
            }
            catch (IdentityOperationFailedException ex)
            {
                return GetErrorResult(ex.IdentityResult);
            }
        }
        [Route("{userId:guid}")]
        public IHttpActionResult PutUser(string userId, PutUserVm model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (userId != model.Id)
                return BadRequest("Ids do not match.");
            try
            {
                var userDto = new UserDto
                {
                    Id = model.Id,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IndexNumber = model.IndexNumber,
                    Pesel = model.Pesel,
                    Role = model.Role
                };
                var user = _userService.UpdateUser(userDto);
                return Ok(user);
            }
            catch (IdentityOperationFailedException ex)
            {
                return GetErrorResult(ex.IdentityResult);
            }
        }

        [Route("{userId:guid}")]
        public IHttpActionResult DeleteUser(string userId)
        {
            try
            {
                _userService.DeleteUser(userId);
                return Ok();
            }
            catch (IdentityOperationFailedException ex)
            {
                return GetErrorResult(ex.IdentityResult);
            }
        }

        [Route("dupa")]
        [HttpPost]
        public IHttpActionResult Dupa()
        {
            return Ok("Dupa");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        [Route("disable")]
        [HttpPost]
        public IHttpActionResult DisableUser(string[] userIds)
        {
            try
            {
                var user = _userService.DisableUser(userIds[0]);
                return Ok(user);
            }
            catch (IdentityOperationFailedException ex)
            {
                return GetErrorResult(ex.IdentityResult);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        [Route("activate")]
        [HttpPost]
        public IHttpActionResult ActivateUser(string[] userIds)
        {
            try
            {
                var user = _userService.ActivateUser(userIds[0]);
                return Ok(user);
            }
            catch (IdentityOperationFailedException ex)
            {
                return GetErrorResult(ex.IdentityResult);
            }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
                return InternalServerError();

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                    return BadRequest();

                return BadRequest(ModelState);
            }

            return null;
        }


    }

}