using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Forex.Models
{
    public class Stock
    {
        public string StockId { get; set; }
        public string StockName { get; set; }

        [JsonIgnore]
        public List<Item> Items { get; set; }
        [JsonIgnore]
        public List<Offer> Offers { get; set; }
    }
}
