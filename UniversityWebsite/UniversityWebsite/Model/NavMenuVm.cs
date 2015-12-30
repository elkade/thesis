using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityWebsite.Model.Page;

namespace UniversityWebsite.Model
{
    public class NavMenuVm
    {
        public NavMenuVm()
        {
           Items = new List<PageMenuItemVm>();
        }
        public List<PageMenuItemVm> Items { get; set; }
        public bool IsTopLevel { get; set; }
    }
}