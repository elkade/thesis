using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using UniversityWebsite.Services;
using UniversityWebsite.Services.Model;

namespace UniversityWebsite.ApiControllers
{
    [RoutePrefix("api/teaching")]
    public class TeachingController : ApiController
    {
        private readonly ISubjectService _subjectService;

        public TeachingController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }
        [Route("subjects")]
        //[AntiForgeryValidate]
        public IEnumerable<SubjectDto> GetSubjects(int? offset = null, int? limit = null)//max limit to 50
        {

            return _subjectService.GetSubjects(offset??0, limit??50);
        }
    }
}