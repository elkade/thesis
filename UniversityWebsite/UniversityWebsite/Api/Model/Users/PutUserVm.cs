using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Api.Model.Users
{
    public class PutUserVm
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(32)]
        public string FirstName { get; set; }
        [MaxLength(32)]
        public string LastName { get; set; }
        [StringLength(6, MinimumLength = 6)]
        public string IndexNumber { get; set; }
        [StringLength(11, MinimumLength = 11)]
        public string Pesel { get; set; }
        public string Role { get; set; }
    }
}