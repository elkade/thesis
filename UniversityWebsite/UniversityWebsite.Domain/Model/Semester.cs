using System.Collections.Generic;

namespace UniversityWebsite.Domain.Model
{
    public class Semester
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}