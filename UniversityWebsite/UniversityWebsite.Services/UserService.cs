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
using System.Collections.Generic;

namespace UniversityWebsite.Services
{
    /// <summary>
    /// Serwis odpowiedzialny za logikę biznesową dotyczącą użytkowników systemu.
    /// </summary>
    public interface IUserService
    {
        IEnumerable<User> GetUsers(int limit, int offset);
        int GetUsersNumber();
        IEnumerable<User> GetUsersByRole(string roleName, int limit, int offset);
        int GetUsersNumberByRole(string roleName);
        //User FindUser(string login);
        //void CreateUser(User user, string password, string role);
        //UserCreateResult CreateUser(User user, string role);
        //User UpdateUser(User user, string role);
        //User GetUser(string userId);

        //void DeleteUser(string userId);

        //void ChangePassword(string userId, string currentPassword, string newPassword);
    }
    /// <summary>
    /// Serwis odpowiedzialny za logikę biznesową dotyczącą użytkowników systemu.
    /// </summary>
    public class UserService : IUserService
    {
        private IDomainContext _context;
        private ApplicationUserManager _userManager;
        //private RoleManager<IdentityRole> _roleManager;

        ///// <summary>
        ///// Tworzy nową instancję serwisu.
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="userManager"></param>
        ///// <param name="roleManager"></param>
        public UserService(IDomainContext context, ApplicationUserManager userManager/*, RoleManager<IdentityRole> roleManager*/)
        {
            _context = context;
            _userManager = userManager;
            // _roleManager = roleManager;

            //_userManager.PasswordValidator = new PasswordValidator
            //{
            //    RequiredLength = 8,
            //    RequireNonLetterOrDigit = true,
            //    RequireDigit = true,
            //    RequireLowercase = true,
            //    RequireUppercase = true,
            //};
        }
        ///// <summary>
        ///// Zwraca dane użytkownika o podanym loginie, lub null jeżeli użytkownik nie istnieje.
        ///// </summary>
        ///// <param name="login">login użytkownika</param>
        ///// <returns>dane użytkownika lub null</returns>
        //public User FindUser(string login)
        //{
        //   return _context.Users.FirstOrDefault(user => user.UserName == login);
        //}

        //public void CreateUser(User user, string password, string role)
        //{
        //    var createResult = _userManager.Create(user, password);
        //    if (createResult.Succeeded)
        //    {
        //        var addToRoleResult = _userManager.AddToRole(user.Id, role);
        //        if(!addToRoleResult.Succeeded)
        //            throw new Exception(addToRoleResult.Errors.First());
        //    }
        //    else throw new Exception(createResult.Errors.First());
        //}

        //public UserCreateResult CreateUser(User user, string role)
        //{
        //    string password = GeneratePassword();
        //    user.Id = null;
        //    var createResult = _userManager.Create(user, password);
        //    if (createResult.Succeeded)
        //        _userManager.AddToRole(user.Id, role);
        //    else throw new Exception(createResult.Errors.FirstOrDefault());
        //    return new UserCreateResult
        //    {
        //        Email = user.Email,
        //        FirstName = user.FirstName,
        //        Id = user.Id,
        //        IndexNumber = user.IndexNumber,
        //        LastName = user.LastName,
        //        Pesel = user.Pesel,
        //        Password = password,
        //        Role = role
        //    };
        //}

        //public User UpdateUser(User user, string role)
        //{
        //    var dbUser = _userManager.FindById(user.Id);
        //    dbUser.Email = user.Email;
        //    dbUser.FirstName = user.FirstName;
        //    dbUser.IndexNumber = user.IndexNumber;
        //    dbUser.LastName = user.LastName;
        //    dbUser.Pesel = user.Pesel;
        //    _userManager.Update(dbUser);
        //    if(!_userManager.IsInRole(dbUser.Id, role))
        //    {
        //        var roles = _userManager.GetRoles(dbUser.Id).ToArray();
        //        _userManager.RemoveFromRoles(dbUser.Id, roles);
        //        _userManager.AddToRole(dbUser.Id, role);
        //    }
        //    return _userManager.FindById(user.Id);
        //}

        //private string GeneratePassword()
        //{
        //    Random r = new Random();
        //    string password = Membership.GeneratePassword(5, 2) + (char)('A'+r.Next(26)) + (char)('a'+r.Next(26)) + (char)('0'+r.Next(10));
        //    return password;
        //}

        //public User GetUser(string userId)
        //{
        //    return _userManager.FindById(userId);
        //}


        //public void DeleteUser(string userId)
        //{
        //    var user = _userManager.FindById(userId);
        //    if (user == null)
        //        throw new NotFoundException("User with id: "+userId);
        //    _userManager.Delete(user);
        //}

        //public void ChangePassword(string userId, string currentPassword, string newPassword)
        //{
        //    var result = _userManager.ChangePassword(userId, currentPassword, newPassword);
        //    if (!result.Succeeded)
        //        throw new Exception(result.Errors.FirstOrDefault());
        //}
        public int GetUsersNumber()
        {
            var suLogin = _userManager.SuperUserLogin;

            return
                _context.Users
                    .Count(u => u.Email != suLogin);
        }

        public IEnumerable<User> GetUsersByRole(string roleName, int limit, int offset)
        {
            var role = _context.Roles.SingleOrDefault(r => r.Name == roleName);
            if (role == null)
                return Enumerable.Empty<User>();
            var roleId = role.Id;

            var suLogin = _userManager.SuperUserLogin;

            return
                _context.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId))
                    .Where(u => u.Email != suLogin)
                    .OrderBy(u => u.LastName)
                    .Skip(offset)
                    .Take(limit);
        }

        public IEnumerable<User> GetUsers(int limit, int offset)
        {
            var suLogin = _userManager.SuperUserLogin;

            return
                _context.Users
                    .Where(u => u.Email != suLogin)
                    .OrderBy(u => u.LastName)
                    .Skip(offset)
                    .Take(limit);
        }

        public int GetUsersNumberByRole(string roleName)
        {
            var role = _context.Roles.SingleOrDefault(r => r.Name == roleName);
            if (role == null)
                return 0;
            var roleId = role.Id;

            var suLogin = _userManager.SuperUserLogin;

            return
                _context.Users.Where(u => u.Email != suLogin).Count(u => u.Roles.Any(r => r.RoleId == roleId));

        }
    }
}
