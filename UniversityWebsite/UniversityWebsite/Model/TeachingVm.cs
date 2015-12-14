using System.Collections.Generic;
using UniversityWebsite.Model.Page;
namespace UniversityWebsite.Model
{
    public class TeachingVm
    {
        public int SemestersCount { get; set; }
        public TeachingVm()
        {
            Siblings = new List<PageMenuItemVm>();
        }
        public List<PageMenuItemVm> Siblings { get; set; } 

    }
}