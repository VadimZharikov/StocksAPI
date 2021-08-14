using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Forex.Models
{
    public class Trade
    {
        [Key]
        public int TradeId { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "Decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [ForeignKey("SellerOffer")]
        public int? SellerOfferId { get; set; }

        [ForeignKey("BuyerOffer")]
        public int? BuyerOfferId { get; set; }
        [JsonIgnore]
        public Offer BuyerOffer { get; set; }
        [JsonIgnore]
        public Offer SellerOffer { get; set; }
    }
}
