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
        [Required]
        public virtual Student Student { get; set; }
        [Required]
        public virtual Subject Subject { get; set; }
        [Required]
        public RequestStatus Status { get; set; }

        public void Approve()
        {
            Status = RequestStatus.Approved;
            Subject.Students.Add(Student);
        }

    }
}