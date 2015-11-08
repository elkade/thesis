using UniversityWebsite.Core;
using System.Linq;
using UniversityWebsite.Domain;

namespace UniversityWebsite.Services
{
    public class UserService
    {
        private IDomainContext _context;
        private ApplicationUserManager _userManager;

        public UserService(IDomainContext context, ApplicationUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ApplicationUser FindUser(string login)
        {
                      
           return _context.Users.FirstOrDefault(user => user.UserName == login);
        }
    }
}
