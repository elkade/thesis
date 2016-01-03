using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace UniversityWebsite.Domain.Model
{
    /// <summary>
    /// Reprezentuje użytkownika systemu.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        /// <summary>
        /// Tworzy nową instancję użytkownika.
        /// </summary>
        public User()
        {
            HasForumAccount = false;
        }
        /// <summary>
        /// Imię użytkownika.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Nazwisko użytkownika.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Numer indeksu użytkownika.
        /// </summary>
        [MinLength(6), MaxLength(6)]
        public string IndexNumber { get; set; }
        /// <summary>
        /// Numer PESEL użytkownika.
        /// </summary>
        [MinLength(11), MaxLength(11)]
        public string Pesel { get; set; }
        /// <summary>
        /// Informacja, czy pomyślnie przypisano użytkownikowi konto na forum.
        /// </summary>
        public bool HasForumAccount { get; set; }
    }

}