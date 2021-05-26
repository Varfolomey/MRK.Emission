using MRK.Emission.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MRK.Emission.Domain.Models
{
    [Table("CISTABLE")]
    public class CisInfo
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(188)]
        public string code { get; set; }

        [Required]
        [StringLength(14)]
        public string gtin { get; set; }

        public CISReleaseType releaseType { get; set; }

        [Required]
        [StringLength(44)]
        public string cis { get; set; }

        [ConcurrencyCheck]
        public CISStatus cisStatus { get; set; }
        public string orderId { get; set; }
        [JsonIgnore]
        public string clientName { get; set; }
    }
}
