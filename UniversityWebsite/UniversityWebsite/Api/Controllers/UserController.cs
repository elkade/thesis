using System.Collections.Generic;
using System.Web.Http;
using UniversityWebsite.Api.Model.Users;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Core;
using Microsoft.AspNet.Identity;
using System.Linq;
using UniversityWebsite.Filters;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Api.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("")]
        [Limit(50),Offset]
        public IHttpActionResult GetUsers(string role=null, int? limit = null, int? offset = null)
        {
            IEnumerable<UserDto> users = role == null ? 
                _userService.GetUsers(limit.Value, offset.Value) :
                _userService.GetUsersByRole(role, limit.Value, offset.Value);

            return Ok(users.ToList());
        }

        [Route("count")]
        [Limit(50), Offset]
        public IHttpActionResult GetUsersNumber(string role = null)
        {
            int number = role == null ?
                _userService.GetUsersNumber() :
                _userService.GetUsersNumberByRole(role);

            return Ok(number);
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

        protected IHttpActionResult GetErrorResult(IdentityResult result)
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