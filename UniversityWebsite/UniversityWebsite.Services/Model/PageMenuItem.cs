using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityWebsite.Services.Model
{
    public class PageMenuItem
    {
        public PageMenuItem()
        {
            Children = new List<PageMenuItem>();
        }
        public string Title { get; set; }
        public string UrlName { get; set; }
        public List<PageMenuItem> Children { get; set; }
        
    }
}
