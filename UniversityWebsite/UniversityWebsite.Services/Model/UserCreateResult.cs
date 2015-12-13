namespace UniversityWebsite.Services.Model
{
    public class UserCreateResult
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }
        public string Pesel { get; set; }
        public string Role { get; set; }
    }
}
