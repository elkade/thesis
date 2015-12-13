using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Api.Model.Users
{
    public class UserVm
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }
        public string Pesel { get; set; }
        public string Role { get; set; }
    }
}