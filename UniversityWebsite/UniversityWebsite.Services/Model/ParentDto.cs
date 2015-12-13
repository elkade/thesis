using UniversityWebsite.Services.Validation;

namespace UniversityWebsite.Services.Model
{
    public class ParentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [UrlParameter]
        public string UrlName { get; set; }
    }
}
