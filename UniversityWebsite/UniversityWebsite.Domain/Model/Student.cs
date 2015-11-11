using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    [Table("Students")]
    public class Student : ApplicationUser
    {
        public virtual ICollection<Subject> AssignedSubjects { get; set; }
    }
}