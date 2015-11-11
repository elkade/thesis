using System.ComponentModel.DataAnnotations;
using UniversityWebsite.Domain.Enums;

namespace UniversityWebsite.Domain.Model
{
    public class SignUpRequest
    {
        public SignUpRequest()
        {
            Status = RequestStatus.Submitted;
        }

        [Key]
        public int Id { get; set; }
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
        public RequestStatus Status { get; set; }

        public void Approve()
        {
            Status = RequestStatus.Approved;
            Subject.Students.Add(Student);
        }

    }
}