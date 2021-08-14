using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Forex.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        [JsonIgnore]
        public Wallet Wallet { get; set; }
        [JsonIgnore]
        public List<Item> Items { get; set; }
        [JsonIgnore]
        public List<Offer> Offers { get; set; }
    }
}
