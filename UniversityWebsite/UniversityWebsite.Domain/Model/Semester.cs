using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    public class Semester
    {
        public int Id { get; set; }
        [StringLength(256)]
        public string Description { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}