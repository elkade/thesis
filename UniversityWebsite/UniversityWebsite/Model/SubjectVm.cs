using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityWebsite.Model
{
    public class SubjectVm
    {
        public string Name { get; set; }
        public int SemesterNumber { get; set; }
        public List<string> Teachers { get; set; }

        //public List<> News { get; set; }
        public string Syllabus { get; set; }
        public string Schedule { get; set; }
        public FilesSectionVm Files { get; set; }
    }

    public class FilesSectionVm
    {
    }
}