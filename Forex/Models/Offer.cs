using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Forex.Models
{
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }
        public int Quantity { get; set; }
        public int StocksLeft { get; set; }

        [Column(TypeName = "Decimal(18,2)")]
        public decimal Price { get; set; }
        public string OfferType { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        [ForeignKey("Stock")]
        public string StockId { get; set; }
        [JsonIgnore]
        public Stock Stock { get; set; }

        [JsonIgnore]
        [InverseProperty("BuyerOffer")]
        public Trade BuyersTrade { get; set; }
        [JsonIgnore]
        [InverseProperty("SellerOffer")]
        public Trade SellerTrade { get; set; }
    }
}
