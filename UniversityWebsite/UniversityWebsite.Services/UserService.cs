using System;
using System.Web.Security;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using UniversityWebsite.Core;
using System.Linq;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Exceptions;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    /// <summary>
    /// Serwis odpowiedzialny za logikę biznesową dotyczącą użytkowników systemu.
    /// </summary>
    public interface IUserService
    {
        User FindUser(string login);
        void CreateUser(User user, string password, string role);
        UserCreateResult CreateUser(User user, string role);
        User GetUser(string userId);
    }
    /// <summary>
    /// Serwis odpowiedzialny za logikę biznesową dotyczącą użytkowników systemu.
    /// </summary>
    public class UserService : IUserService
    {
        private IDomainContext _context;
        private ApplicationUserManager _userManager;
        private RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Tworzy nową instancję serwisu.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public UserService(IDomainContext context, ApplicationUserManager userManager/*, RoleManager<IdentityRole> roleManager*/)
        {
            _context = context;
            _userManager = userManager;
           // _roleManager = roleManager;

            _userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
        }
        /// <summary>
        /// Zwraca dane użytkownika o podanym loginie, lub null jeżeli użytkownik nie istnieje.
        /// </summary>
        /// <param name="login">login użytkownika</param>
        /// <returns>dane użytkownika lub null</returns>
        public User FindUser(string login)
        {
           return _context.Users.FirstOrDefault(user => user.UserName == login);
        }

        public void CreateUser(User user, string password, string role)
        {
            ValidateRole(role);
            var createResult = _userManager.Create(user, password);
            if (createResult.Succeeded)
            {
                var addToRoleResult = _userManager.AddToRole(user.Id, role);
                if(!addToRoleResult.Succeeded)
                    throw new Exception(addToRoleResult.Errors.First());
            }
            else throw new Exception(createResult.Errors.First());
        }

        public UserCreateResult CreateUser(User user, string role)
        {
            ValidateRole(role);
            string password = Membership.GeneratePassword(8, 2);
            var createResult = _userManager.Create(user, password);
            if (createResult.Succeeded)
                _userManager.AddToRole(user.Id, role);
            else throw new Exception(createResult.Errors.First());
            return new UserCreateResult
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                IndexNumber = user.IndexNumber,
                LastName = user.LastName,
                Pesel = user.Pesel,
                Password = password,
                Role = role
            };
        }

        public User GetUser(string userId)
        {
            return _userManager.FindById(userId);
        }

        private void ValidateRole(string role)
        {
            //if (!_roleManager.RoleExists(role))
            //    throw new PropertyValidationException("role","Role " + role + " does not exist in system.");
        }
    }
}
