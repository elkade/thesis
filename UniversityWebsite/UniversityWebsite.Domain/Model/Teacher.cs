using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    [Table("Teachers")]
    public class Teacher : User
    {
        public virtual ICollection<Subject> OwnedSubjects { get; set; }
    }
}