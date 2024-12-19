using LingoScape.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LingoScape.DataAccessLayer.Models
{
    /// <summary>
    /// For Dynamically changing texts.
    /// Example. Your new task is to kill {number} {Mob}
    /// </summary>
    [Table("DynamicTranslation")]
    public class DynamicTranslationModel
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

        // This is possibly not needed.
        [Required]
        public TranslationType Type { get; set; }

        [Required]
        public required string Contributor { get; set; }
    }
}
