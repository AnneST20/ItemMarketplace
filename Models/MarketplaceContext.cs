using Microsoft.EntityFrameworkCore;

namespace ItemMarketplace.Models
{
    public class MarketplaceContext : DbContext
    {
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Auction> Auctions { get; set; }

        public MarketplaceContext(DbContextOptions<MarketplaceContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }

}
