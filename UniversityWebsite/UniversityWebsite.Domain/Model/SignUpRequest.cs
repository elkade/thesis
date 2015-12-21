using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversityWebsite.Domain.Enums;

namespace UniversityWebsite.Domain.Model
{
    public class SignUpRequest
    {
        public SignUpRequest(int subjectId, string studentId)
        {
            SubjectId = subjectId;
            StudentId = studentId;
            Status = RequestStatus.Submitted;
            CreateTime = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey("StudentId")]
        public virtual User Student { get; set; }
        public string StudentId { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        public int SubjectId { get; set; }

        [Required]
        public RequestStatus Status { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        public void Approve()
        {
            Status = RequestStatus.Approved;
            Subject.Students.Add(Student);
        }

        public void Refuse()
        {
            Status = RequestStatus.Refused;
        }
    }
}