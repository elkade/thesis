using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace UniversityWebsite.Model.Page
{
    public class PageViewModel
    {
        public PageViewModel()
        {
            Siblings = new List<PageMenuItemVm>();
        }
        public string Title { get; set; }
        public string CountryCode { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string ParentName { get; set; }
        public List<PageMenuItemVm> Siblings { get; set; } 

    }
}