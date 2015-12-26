using System;
using Microsoft.AspNet.Identity;
using UniversityWebsite.Core;
using System.Linq;
using UniversityWebsite.Domain.Model;
using UniversityWebsite.Services.Exceptions;
using System.Collections.Generic;
using UniversityWebsite.Services.Helpers;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Services
{
    /// <summary>
    /// Serwis odpowiedzialny za logikę biznesową dotyczącą użytkowników systemu.
    /// </summary>
    public interface IUserService
    {
        IEnumerable<UserDto> GetUsers(int limit, int offset);
        int GetUsersNumber();
        IEnumerable<UserDto> GetUsersByRole(string roleName, int limit, int offset);
        int GetUsersNumberByRole(string roleName);
        ///// <summary>
        ///// Zwraca dane użytkownika o podanym id, lub null jeżeli użytkownik nie istnieje.
        ///// </summary>
        ///// <param name="login">login użytkownika</param>
        ///// <returns>dane użytkownika lub null</returns>
        UserDto GetUser(string login);
        UserWithPasswordDto CreateUser(UserDto user);
        UserDto UpdateUser(UserDto user);

        void DeleteUser(string userId);

        //void ChangePassword(string userId, string currentPassword, string newPassword);
    }
    /// <summary>
    /// Serwis odpowiedzialny za logikę biznesową dotyczącą użytkowników systemu.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IDomainContext _context;
        private readonly ApplicationUserManager _userManager;
        private readonly ModelFactory _modelFactory;
        ///// <summary>
        ///// Tworzy nową instancję serwisu.
        ///// </summary>
        ///// <param name="userManager"></param>
        public UserService(IDomainContext context, ApplicationUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
            _modelFactory = new ModelFactory(_userManager);
        }
        public UserDto GetUser(string userId)
        {
            var user = _userManager.FindById(userId);
            return _modelFactory.GetDto(user);
        }

        public UserWithPasswordDto CreateUser(UserDto userDto)
        {
            return _context.InTransaction(() =>
            {
                string password = PasswordGenerator.GeneratePassword(8);

                var user = new User
                {
                    Email = userDto.Email,
                    UserName = userDto.Email,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    IndexNumber = userDto.IndexNumber,
                    Pesel = userDto.Pesel,
                };

                var result = _userManager.Create(user, password);
                if (!result.Succeeded)
                    throw new IdentityOperationFailedException(result);

                result = _userManager.AddToRole(user.Id, userDto.Role);
                if (!result.Succeeded)
                    throw new IdentityOperationFailedException(result);

                return _modelFactory.GetDtoWithPassword(user, password);
            });
        }

        public UserDto UpdateUser(UserDto userDto)
        {
            return _context.InTransaction(() =>
            {
                var dbUser = _userManager.FindById(userDto.Id);
                if (dbUser == null)
                    throw new NotFoundException("User with id: " + userDto.Id);
                dbUser.Email = userDto.Email;
                dbUser.UserName = userDto.Email;
                dbUser.FirstName = userDto.FirstName;
                dbUser.IndexNumber = userDto.IndexNumber;
                dbUser.LastName = userDto.LastName;
                dbUser.Pesel = userDto.Pesel;

                var result = _userManager.Update(dbUser);

                if (!result.Succeeded)
                    throw new IdentityOperationFailedException(result);

                if (!_userManager.IsInRole(userDto.Id,userDto.Role))
                {
                    var roles = _userManager.GetRoles(userDto.Id).ToArray();
                    result = _userManager.RemoveFromRoles(userDto.Id, roles);
                    if (!result.Succeeded)
                        throw new IdentityOperationFailedException(result);
                    result = _userManager.AddToRole(userDto.Id, userDto.Role);
                    if (!result.Succeeded)
                        throw new IdentityOperationFailedException(result);
                }
                return _modelFactory.GetDto(dbUser);
            });
        }


        public void DeleteUser(string userId)
        {
            var suId =_userManager.SuperUserId;

            if(userId == suId)
                throw new InvalidOperationException("Cannot delete superuser");
            var user = _userManager.FindById(userId);
            if (user == null)
                throw new NotFoundException("User with id: "+userId+" does not exist.");
            var result = _userManager.Delete(user);
            if (!result.Succeeded)
                throw new IdentityOperationFailedException(result);
        }

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

        public IEnumerable<UserDto> GetUsersByRole(string roleName, int limit, int offset)
        {
            var role = _context.Roles.SingleOrDefault(r => r.Name == roleName);
            if (role == null)
                return Enumerable.Empty<UserDto>();
            var roleId = role.Id;

            var suLogin = _userManager.SuperUserLogin;

            return
                _context.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId))
                    .Where(u => u.Email != suLogin)
                    .OrderBy(u => u.LastName)
                    .Skip(offset)
                    .Take(limit)
                    .Select(_modelFactory.GetDto);
        }

        public IEnumerable<UserDto> GetUsers(int limit, int offset)
        {
            var suLogin = _userManager.SuperUserLogin;

            return
                _context.Users
                    .Where(u => u.Email != suLogin)
                    .OrderBy(u => u.LastName)
                    .Skip(offset)
                    .Take(limit)
                    .Select(_modelFactory.GetDto);
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
        public class ModelFactory
        {
            private readonly ApplicationUserManager _userManager;

            public ModelFactory(ApplicationUserManager userManager)
            {
                _userManager = userManager;
            }

            public UserDto GetDto(User appUser)
            {
                var roles = _userManager.GetRoles(appUser.Id);
                string role = null;
                if (roles.Count > 0)
                    role = roles[0];
                return new UserDto
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
            public UserWithPasswordDto GetDtoWithPassword(User appUser, string password)
            {
                var roles = _userManager.GetRoles(appUser.Id);
                string role = null;
                if (roles.Count > 0)
                    role = roles[0];
                return new UserWithPasswordDto
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
}
