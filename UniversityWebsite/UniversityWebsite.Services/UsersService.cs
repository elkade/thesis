using UniversityWebsite.Core;
using System.Linq;
using UniversityWebsite.Domain;
using UniversityWebsite.Domain.Model;

namespace UniversityWebsite.Services
{
    /// <summary>
    /// Serwis odpowiedzialny za logikę biznesową dotyczącą użytkowników systemu.
    /// </summary>
    public class UserService
    {
        private IDomainContext _context;
        private ApplicationUserManager _userManager;
        /// <summary>
        /// Tworzy nową instancję serwisu.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public UserService(IDomainContext context, ApplicationUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
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
    }
}
