using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Forex.Models
{
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Funds { get; set; }
    }
}
