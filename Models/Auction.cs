using System;
using System.ComponentModel.DataAnnotations;

namespace ItemMarketplace.Models
{
    public class Auction
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public DateTime CreatedDb { get; set; }
        public DateTime FinishedDb { get; set; }
        public decimal Price { get; set; }
        [Range (0, 3)] // none = 0, canceled = 1, finished = 2, active = 3
        public MarketStatus Status { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }

        public virtual Item Item { get; set; } // for filtration to get item's name
    }

    public enum MarketStatus
    {
        None,
        Canceled,
        Finished,
        Active
    }
}
