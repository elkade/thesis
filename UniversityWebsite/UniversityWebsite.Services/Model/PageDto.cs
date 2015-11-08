using System;

namespace UniversityWebsite.Services.Model
{
    public class PageDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UrlName { get; set; }
        public string Content { get; set; }
        public int LangGroup { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string CountryCode { get; set; }
        public string ParentUrlName { get; set; }
    }
}
