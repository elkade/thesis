using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    [Table("Students")]
    public class Student : User
    {
        public virtual ICollection<Subject> AssignedSubjects { get; set; }
    }
}