using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    /// <summary>
    /// Reprezemtuje grupę stron.
    /// </summary>
    public class PageGroup
    {
        /// <summary>
        /// Id obiektu bazie danych.
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Kolekcja stron, należących do danej grupy.
        /// </summary>
        public virtual ICollection<Page> Pages { get; set; } 
    }
}
