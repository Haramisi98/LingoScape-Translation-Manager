using LingoScape.Shared;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LingoScape.DataAccessLayer.Models
{
    /// <summary>
    /// This Table holds the common translations, with language code.
    /// </summary>
    [Table("StaticTranslation")]
    public class StaticTranslationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string RawText { get; set; }

        /// <summary>
        /// ISO 2 letter country code
        /// </summary>
        [Required]
        public required string LanguageCode { get; set; }

        [Required]
        public required string Translation { get; set; }

        [Required]
        public TranslationType Type { get; set; }

        [Required]
        public required string Contributor { get; set; }
    }
}
