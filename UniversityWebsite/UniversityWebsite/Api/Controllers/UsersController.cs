﻿using System.Web.Http;
using UniversityWebsite.Api.Model.Users;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Core;
using Microsoft.AspNet.Identity;
using System.Linq;
using UniversityWebsite.Services.Helpers;

namespace UniversityWebsite.Api.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly ApplicationUserManager _userManager;

        private readonly ModelFactory _modelFactory;

        public UsersController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
            _userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            _modelFactory = new ModelFactory(_userManager);
        }

        [Route("")]
        public IHttpActionResult GetUsers()
        {
            var login = _userManager.SuperUserLogin;
            return Ok(_userManager.Users.Where(u => u.Email != login).ToList().Select(_modelFactory.GetReturnModel));
        }

        [Route("{userId:guid}")]
        public IHttpActionResult GetUser(string userId)
        {
            var user = _userManager.FindById(userId);
            if (user == null)
                return NotFound();
            return Ok(_modelFactory.GetReturnModel(user));
        }
        [Route("")]
        public IHttpActionResult PostUser(PostUserVm model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var user = new User
	        {
		        UserName = model.Email,
		        Email = model.Email,
		        FirstName = model.FirstName,
		        LastName = model.LastName,
		        IndexNumber = model.IndexNumber,
		        Pesel = model.Pesel,
	        };

            string password = PasswordGenerator.GeneratePassword(8);

            var result = _userManager.Create(user, password);

            if (!result.Succeeded)
                return GetErrorResult(result);

            result = _userManager.AddToRole(user.Id, model.Role);
            if (!result.Succeeded)
                return GetErrorResult(result);


            return Ok(_modelFactory.GetCreatedReturnModel(user, password));
        }
        [Route("{userId:guid}")]
        public IHttpActionResult PutUser(string userId, PutUserVm model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (userId != model.Id)
                return BadRequest("Ids do not match.");

            var user = _userManager.FindById(userId);
            user.UserName = model.Email;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.IndexNumber = model.IndexNumber;
            user.Pesel = model.Pesel;

            var result = _userManager.Update(user);

            if (!result.Succeeded)
                return GetErrorResult(result);

            if (!_userManager.IsInRole(user.Id, model.Role))
            {
                var roles = _userManager.GetRoles(user.Id).ToArray();
                result = _userManager.RemoveFromRoles(user.Id, roles);
                if (!result.Succeeded)
                    return GetErrorResult(result);
                result = _userManager.AddToRole(user.Id, model.Role);
                if (!result.Succeeded)
                    return GetErrorResult(result);
            }

            return Ok(_modelFactory.GetReturnModel(user));
        }


        [Route("{userId:guid}")]
        public IHttpActionResult DeleteUser(string userId)
        {
            var user = _userManager.FindById(userId);
            if (user == null)
                return BadRequest("User does not exist.");
            var result = _userManager.Delete(user);
            if(!result.Succeeded)
                return GetErrorResult(result);
            return Ok();
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
    public class UserReturnModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }
        public string Pesel { get; set; }
        public string Role { get; set; }
    }
    public class UserCreatedReturnModel : UserReturnModel
    {
        public string Password { get; set; }
    }
    public class ModelFactory
    {
        private ApplicationUserManager _AppUserManager;

        public ModelFactory(ApplicationUserManager appUserManager)
        {
            _AppUserManager = appUserManager;
        }

        public UserReturnModel GetReturnModel(User appUser)
        {
            var roles = _AppUserManager.GetRoles(appUser.Id);
            string role = null;
            if (roles.Count > 0)
                role = roles[0];
            return new UserReturnModel
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                IndexNumber = appUser.IndexNumber,
                Pesel = appUser.Pesel,
                Role = role,
            };
        }
        public UserCreatedReturnModel GetCreatedReturnModel(User appUser, string password)
        {
            var roles = _AppUserManager.GetRoles(appUser.Id);
            string role = null;
            if (roles.Count > 0)
                role = roles[0];
            return new UserCreatedReturnModel
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                IndexNumber = appUser.IndexNumber,
                Pesel = appUser.Pesel,
                Role = role,
                Password = password
            };
        }
    }
}