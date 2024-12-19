using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LingoScape.Shared;

namespace LingoScape.DataAccessLayer.Models
{
    /// <summary>
    /// This table is holding all possible texts that can be translated. 
    /// This table is not intented to be used a whole lot other than keeping record of what can be translated.
    /// </summary>
    [Table("Translateable")]
    public class TranslatableTextModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string RawText { get; set; }

        [Required]
        public TranslationType Type { get; set; }

        [Required]
        public bool IsDynamic { get; set; }
    }
}
