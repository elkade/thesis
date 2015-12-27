﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UniversityWebsite.Domain.Enums;

namespace UniversityWebsite.Domain.Model
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(64)]
        public string Name { get; set; }
        [Required, StringLength(96)]
        public string UrlName { get; set; }

        public virtual ICollection<News> News { get; set; }

        public virtual Syllabus Syllabus { get; set; }

        public virtual Schedule Schedule { get; set; }

        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<TeacherSubject> Teachers { get; set; }
        public virtual ICollection<SignUpRequest> SignUpRequests { get; set; }

        [Required]
        public int Semester { get; set; }

        public bool HasStudent(string userId)
        {
            return SignUpRequests.Any(r => r.StudentId == userId && r.Status == RequestStatus.Approved);
        }

        public bool HasTeacher(string userId)
        {
            return Teachers.Any(t=>t.TeacherId == userId);
        }
    }
}