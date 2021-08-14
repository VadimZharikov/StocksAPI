using Microsoft.EntityFrameworkCore;

namespace Forex.Models
{
    public class UserContext : DbContext
    {
        public UserContext()
        {

        }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Trade> Trades { get; set; }
    }
}
