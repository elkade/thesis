using System;
using System.Web.Security;

namespace UniversityWebsite.Services.Helpers
{
    public class PasswordGenerator
    {
        public static string GeneratePassword(int length)
        {
            if(length<5) throw new ArgumentException("length cannot be shorter than 5");
            Random r = new Random();
            string password = Membership.GeneratePassword(length - 3, 2) + (char)('A' + r.Next(26)) + (char)('a' + r.Next(26)) + (char)('0' + r.Next(10));
            return password;
        }
    }
}
