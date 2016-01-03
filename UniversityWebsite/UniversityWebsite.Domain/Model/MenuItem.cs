using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebsite.Domain.Model
{
    /// <summary>
    /// Reprezentuje element menu.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Id obiektu w bazie danych.
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Id strony, której odnośnik zawiera dany element menu.
        /// </summary>
        public int PageId { get; set; }
        /// <summary>
        /// Strona, której odnośnik zawiera dany element menu.
        /// </summary>
        [Required]
        [ForeignKey("PageId")]
        public virtual Page Page { get; set; }

        /// <summary>
        /// URL obrazka, zawartego w elemencie menu.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Id menu, do którego należy dany element.
        /// </summary>
        public int MenuId { get; set; }
        //[Required]
        /// <summary>
        /// Menu, do którego należy dany element.
        /// </summary>
        [ForeignKey("MenuId")]
        public virtual Menu Menu { get; set; }

        /// <summary>
        /// Numer porządkowy elementu w menu.
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// Opis elementu menu.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Tytuł elementu menu.
        /// </summary>
        public string Title { get; set; }
    }
}
