using System.Collections.Generic;

namespace UniversityWebsite.Model.Page
{
    public class PageMenuItemVm
    {
        public PageMenuItemVm()
        {
            Children = new List<PageMenuItemVm>();
        }
        public string Title { get; set; }
        public string UrlName { get; set; }
        public List<PageMenuItemVm> Children { get; set; }
    }
}