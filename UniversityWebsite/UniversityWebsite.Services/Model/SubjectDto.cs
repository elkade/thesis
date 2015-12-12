using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityWebsite.Services.Model
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlName { get; set; }

        public int SemesterNumber { get; set; }

        public List<ArticleDto> News { get; set; }
        public ArticleDto Syllabus { get; set; }
        public ArticleDto Schedule { get; set; }
    }
}
