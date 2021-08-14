using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Forex.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("Stock")]
        public string StockId { get; set; }
        [JsonIgnore]
        public Stock Stock { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
