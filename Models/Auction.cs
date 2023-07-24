using System;

namespace ItemMarketplace.Models
{
    public class Auction
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public DateTime CreatedDb { get; set; }
        public DateTime FinishedDb { get; set; }
        public decimal Price { get; set; }
        public MarketStatus Status { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }

        public virtual Item Item { get; set; }
    }

    public enum MarketStatus
    {
        None,
        Canceled,
        Finished,
        Active
    }
}
