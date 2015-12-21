using System;
using UniversityWebsite.Domain.Enums;

namespace UniversityWebsite.Services.Model
{
    public class SignUpRequestForSubjectVm
    {
        public int Id { get; set; }

        public string StudentId { get; set; }

        public string StudentFirstName { get; set; }

        public string StudentLastName { get; set; }

        public string StudentIndex { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
