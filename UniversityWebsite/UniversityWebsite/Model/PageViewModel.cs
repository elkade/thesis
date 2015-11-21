using System;
using System.Web.Mvc;

namespace UniversityWebsite.Model
{
    public class PageViewModel
    {
        public string Title { get; set; }
        public string CountryCode { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string ParentName { get; set; }
    }
}