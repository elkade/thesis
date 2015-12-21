using System;
using UniversityWebsite.Domain.Enums;

namespace UniversityWebsite.Services.Model
{
    public class SignUpRequestForStudentVm
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }

        public string SubjectName { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
