using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    /// <summary>
    /// Reprezentuje menu.
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Id obiektu w bazie danych.
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Id języka, do którego przypisane jest menu.
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// Język, do którego przypisane jest menu.
        /// </summary>
        [ForeignKey("CountryCode")]
        public virtual Language Language { get; set; }

        /// <summary>
        /// Kolekcja elementów menu.
        /// </summary>
        public virtual ICollection<MenuItem> Items { get; set; }
        /// <summary>
        /// Id grupy menu, w której dane menu się znajduje.
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// Grupa, w której dane menu się znajduje.
        /// </summary>
        [Required]
        [ForeignKey("GroupId")]
        public MenuGroup Group { get; set; }
    }
}