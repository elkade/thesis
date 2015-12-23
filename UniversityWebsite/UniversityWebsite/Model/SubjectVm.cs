using System.Collections.Generic;
using UniversityWebsite.Model.Page;

namespace UniversityWebsite.Model
{
    public class SubjectVm
    {
        public string Name { get; set; }
        public int SemesterNumber { get; set; }
        public List<string> Teachers { get; set; }

        public List<NewsVm> News { get; set; }
        public string Syllabus { get; set; }
        public string Schedule { get; set; }
        public List<FileViewModel> Files { get; set; }

        public SubjectVm()
        {
            Siblings = new List<PageMenuItemVm>();
        }
        public List<PageMenuItemVm> Siblings { get; set; } 
    }

}