﻿using System;
using System.Configuration;
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
        /// <summary>
        /// Zwraca użytkowników systemu.
        /// </summary>
        /// <param name="limit">Maksymalna liczba zwróconych obiektów</param>
        /// <param name="offset">Numer porządkowy pierwszego obiektu, który ma zostać zwrócony</param>
        /// <returns>Zbiór obiektów reprezentujących użytkowników systemu.</returns>
        IEnumerable<UserDto> GetUsers(int limit, int offset);
        /// <summary>
        /// Zwraca liczbę użytkowników systemu.
        /// </summary>
        /// <returns>Liczba naturalna.</returns>
        int GetUsersNumber();
        /// <summary>
        /// Zwraca użytkowników systemu o danej roli.
        /// </summary>
        /// <param name="roleName">Nazwa roli</param>
        /// <param name="limit">Maksymalna liczba zwróconych obiektów</param>
        /// <param name="offset">Numer porządkowy pierwszego obiektu, który ma zostać zwrócony</param>
        /// <returns>Zbiór obiektów reprezentujących użytkowników systemu.</returns>
        IEnumerable<UserDto> GetUsersByRole(string roleName, int limit, int offset);
        /// <summary>
        /// Zwraca liczbę użytkowników systemu o danej roli.
        /// </summary>
        /// <param name="roleName">Nazwa roli.</param>
        /// <returns>Liczba naturalna.</returns>
        int GetUsersNumberByRole(string roleName);
        /// <summary>
        /// Zwraca dane użytkownika o podanym id, lub null jeżeli użytkownik nie istnieje.
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <returns>Obiekt reprezentujący użytkownika systemu.</returns>
        UserDto GetUser(string userId);
        /// <summary>
        /// Tworzy nowegu użytkownika na podstawie danych wejściowych i zapiuje go w bazie danych.
        /// </summary>
        /// <param name="user">Dane użytkownika</param>
        /// <returns>Dane użytkownika umieszczone w bazie danych.</returns>
        UserWithPasswordDto CreateUser(UserDto user);
        /// <summary>
        /// Aktualizuje dane użytkownika.
        /// </summary>
        /// <param name="user">Dane użytkownika</param>
        /// <returns>Obiekt reprezentujący zaktualizowane dane użytkownika.</returns>
        UserDto UpdateUser(UserDto user);
        /// <summary>
        /// Usuwa użytkownika z systemu.
        /// </summary>
        /// <param name="userId">Id użytkownika do usunięcia</param>
        void DeleteUser(string userId);
        /// <summary>
        /// Deaktywuje użytkownika
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <returns></returns>
        UserDto DisableUser(string userId);
        /// <summary>
        /// Aktywuje deaktywowanego użytkownika
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <returns></returns>
        UserDto ActivateUser(string userId);
    }
    /// <summary>
    /// Serwis odpowiedzialny za logikę biznesową dotyczącą użytkowników systemu.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly string _hostName = ConfigurationManager.AppSettings["hostName"];
        private readonly IDomainContext _context;
        private readonly ApplicationUserManager _userManager;
        private readonly ModelFactory _modelFactory;
        /// <summary>
        /// Tworzy nową instancję serwisu.
        /// </summary>
        /// <param name="context">Kontekst domeny systemu.</param>
        /// <param name="userManager">Manaer bezpośrednio zarządzający użytkownikami systemu.</param>
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
            User user = new User();
            var createdUser = _context.InTransaction(() =>
            {
                string password = PasswordGenerator.GeneratePassword(8);

                user = new User
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
            using (var client = new ForumUserClient(_hostName))
                user.HasForumAccount = client.TryAddUser(createdUser.Email, createdUser.Password, createdUser.Role == "Administrator");
            _context.Users.Attach(user);
            _context.SetPropertyModified(user, u=>u.HasForumAccount);
            _context.SaveChanges();
            createdUser.HasForumAccount = user.HasForumAccount;
            return createdUser;
        }

        public UserDto UpdateUser(UserDto userDto)
        {
            bool? isAdmin = null;
            bool hasForumAccount = false;
            string oldMail=null;
            var user = _context.InTransaction(() =>
            {
                var dbUser = _userManager.FindById(userDto.Id);
                if (dbUser == null)
                    throw new NotFoundException("User with id: " + userDto.Id);
                oldMail = dbUser.Email;
                dbUser.Email = userDto.Email;
                dbUser.UserName = userDto.Email;
                dbUser.FirstName = userDto.FirstName;
                dbUser.IndexNumber = userDto.IndexNumber;
                dbUser.LastName = userDto.LastName;
                dbUser.Pesel = userDto.Pesel;
                hasForumAccount = dbUser.HasForumAccount;
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

                    isAdmin = userDto.Role == "Administrator";
                }
                return _modelFactory.GetDto(dbUser);
            });
            if(isAdmin.HasValue && hasForumAccount)
                using (var client = new ForumUserClient(_hostName))
                    client.TryEditUser(oldMail, isAdmin.Value, user.Email);
            return user;
        }


        public void DeleteUser(string userId)
        {
            var suId =_userManager.SuperUserId;

            if(userId == suId)
                throw new InvalidOperationException("Cannot delete superuser");
            var user = _userManager.FindById(userId);
            if (user == null)
                throw new NotFoundException("User with id: "+userId+" does not exist.");
            bool hasForumAccount = user.HasForumAccount;
            string email = user.Email;
            var result = _userManager.Delete(user);
            if (!result.Succeeded)
                throw new IdentityOperationFailedException(result);
            if (!hasForumAccount) return;
            using (var client = new ForumUserClient(_hostName))
                client.TryDeleteUser(email);
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

        public UserDto DisableUser(string userId)
        {
            var user = _context.InTransaction(() =>
            {
                var dbUser = _userManager.FindById(userId);
                if (dbUser == null)
                    throw new NotFoundException("User with id: " + userId);
                dbUser.LockoutEndDateUtc = DateTime.Now.AddYears(1000);

                var result = _userManager.Update(dbUser);

                if (!result.Succeeded)
                    throw new IdentityOperationFailedException(result);

                return _modelFactory.GetDto(dbUser);
            });

            return user;
        }
        public UserDto ActivateUser(string userId)
        {
            var user = _context.InTransaction(() =>
            {
                var dbUser = _userManager.FindById(userId);
                if (dbUser == null)
                    throw new NotFoundException("User with id: " + userId);
                var password = PasswordGenerator.GeneratePassword(8);
                var token = _userManager.GeneratePasswordResetToken(userId);
                var result = _userManager.ResetPassword(userId, token, password);

                if (!result.Succeeded)
                    throw new IdentityOperationFailedException(result);

                dbUser.LockoutEndDateUtc = null;

                result = _userManager.Update(dbUser);

                if (!result.Succeeded)
                    throw new IdentityOperationFailedException(result);

                return _modelFactory.GetDtoWithPassword(dbUser, password);
            });

            return user;
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
                    Disabled = appUser.LockoutEndDateUtc > DateTime.Now
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
                    Password = password,
                    Disabled = appUser.LockoutEndDateUtc > DateTime.Now
                };
            }
        }

    }
}
