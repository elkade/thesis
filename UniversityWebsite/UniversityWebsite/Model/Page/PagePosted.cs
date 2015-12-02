using System.ComponentModel.DataAnnotations;
using UniversityWebsite.Services.Validation;

namespace UniversityWebsite.Model.Page
{
    public class PagePosted
    {
        [Required]
        public string Title { get; set; }
        [UrlParameter]
        public string UrlName { get; set; }
        public string Content { get; set; }
        public int? GroupId { get; set; }
        [Required]
        public string CountryCode { get; set; }
        public int? ParentId { get; set; }
        public bool IsTile { get; set; }
        public string Description { get; set; }
    }
}