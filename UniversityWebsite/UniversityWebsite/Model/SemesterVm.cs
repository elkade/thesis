using System.Collections.Generic;

namespace UniversityWebsite.Model
{
    public class SemesterVm
    {
        public List<SubjectListElementVm> Subjects { get; set; }
    }

    public class SubjectListElementVm
    {
        public string SubjectName { get; set; }
    }
}