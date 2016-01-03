using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Domain.Model
{
    /// <summary>
    /// Reprezentuje grupę menu.
    /// </summary>
    public class MenuGroup
    {
        /// <summary>
        /// Id obiektu w bazie danych.
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}
