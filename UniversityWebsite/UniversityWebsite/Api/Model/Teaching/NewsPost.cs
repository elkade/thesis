using System.ComponentModel.DataAnnotations;

namespace UniversityWebsite.Api.Model.Teaching
{
    /// <summary>
    /// Reprezentuje dane nowego wpisu w aktualnościach
    /// </summary>
    public class NewsPost
    {
        /// <summary>
        /// Nagłówek wpisu
        /// </summary>
        [Required]
        public string Header { get; set; }
        /// <summary>
        /// Zawartość wpisu
        /// </summary>
        [Required]
        public string Content { get; set; }
    }
}