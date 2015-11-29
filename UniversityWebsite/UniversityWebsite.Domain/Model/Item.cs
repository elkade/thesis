using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    public abstract class Item
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public abstract string Link { get; }
    }
}
