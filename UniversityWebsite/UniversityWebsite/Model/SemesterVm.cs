using System.Collections.Generic;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.Model
{
    public class SemesterVm
    {
        public List<SubjectListElementVm> Subjects { get; set; }
    }

    public class SubjectListElementVm
    {
        public string SubjectName { get; set; }
        public string SubjectUrlName { get; set; }
        public SignUpAction SignUpAction { get; set; }
        public int SubjectId { get; set; }
    }

}