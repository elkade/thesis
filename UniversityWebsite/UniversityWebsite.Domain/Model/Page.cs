using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    /// <summary>
    /// Reprezentuje stronę systemu.
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Id obiektu w bazie danych.
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Tytul strony.
        /// </summary>
        [Required]
        [StringLength(64)]
        public string Title { get; set; }
        /// <summary>
        /// Tytuł strony w adresie URL.
        /// </summary>
        [Required]
        [StringLength(64)]
        public string UrlName { get; set; }
        /// <summary>
        /// Zawartość strony.
        /// </summary>
        [Column(TypeName = "text")]
        public string Content { get; set; }
        /// <summary>
        /// Data stworzenia strony.
        /// </summary>
        [Required]
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// Data ostatniej modyfikacji strony.
        /// </summary>
        [Required]
        public DateTime LastUpdateDate { get; set; }
        /// <summary>
        /// Id języka, do którego przypisana jest strona.
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// Język, do którego przypisana jest strona.
        /// </summary>
        [Required]
        [ForeignKey("CountryCode")]
        public Language Language { get; set; }
        /// <summary>
        /// Id grupy stron, do której przypisana jest strona.
        /// </summary>
        [Required]
        public int GroupId { get; set; }
        /// <summary>
        /// Grupa, do której przypisana jest strona.
        /// </summary>
        [Required]
        [ForeignKey("GroupId")]
        public virtual PageGroup Group { get; set; }
        /// <summary>
        /// Id strony będącej rodzicem danej.
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// Strona będąca rodzicem danej.
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual Page Parent { get; set; }
        /// <summary>
        /// Opis strony.
        /// </summary>
        public string Description { get; set; }
    }
}