using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ItemMarketplace.Models;

namespace ItemMarketplace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly MarketplaceContext _context;

        public AuctionsController(MarketplaceContext context)
        {
            _context = context;
        }

        // GET: api/Auctions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions(string name = null, string status = null, string seller = null, string sort_order = null, string sort_key = null)
        {
            var auctions =  await _context.Auctions.Include(a => a.Item).ToListAsync();
            if (auctions.Count != 0)
            {
                if (name != null)
                {
                    auctions = FilterByName(auctions, name); // name is an item's name (auction doesn't contain it)
                }
                if (seller != null)
                {
                    auctions = FilterBySeller(auctions, seller);
                }
                if (status != null)
                {
                    try
                    {
                        auctions = FilterByStatus(auctions, status);
                    }
                    catch (Exception ex) { }
                }
                auctions = Sort(auctions, sort_key, sort_order); // by default sorted by id asc
            }

            return Ok(auctions);
        }

        // GET: api/Auctions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Auction>> GetAuction(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);

            if (auction == null)
            {
                return NotFound();
            }

            return auction;
        }

        private List<Auction> FilterByName(IEnumerable<Auction> auctions, string name)
        {
            name = name.ToLower();
            return auctions.Where(a => a.Item.Name.ToLower().Equals(name)).ToList();
        }

        private List<Auction> FilterBySeller(IEnumerable<Auction> auctions, string seller)
        {
            seller = seller.ToLower();
            return auctions.Where(a => a.Seller.ToLower().Equals(seller)).ToList();
        }

        private List<Auction> FilterByStatus(IEnumerable<Auction> auctions, string status)
        {
            status = status.ToLower();
            var mStatus = new MarketStatus();

            switch (status)
            {
                case "none":
                    mStatus = MarketStatus.None;
                    break;
                case "active":
                    mStatus = MarketStatus.Active;
                    break;
                case "canceled":
                    mStatus = MarketStatus.Canceled;
                    break;
                case "finished":
                    mStatus = MarketStatus.Finished;
                    break;
                default:
                    throw new Exception();

            }

            return auctions.Where(a => a.Status == mStatus).ToList();
        }

        private List<Auction> Sort(IEnumerable<Auction> auctions, string sort_key, string sort_order)
        {
            switch (sort_order)
            {
                case "desc":
                    {
                        if (sort_key != null)
                            sort_key = sort_key.ToLower();

                        switch (sort_key)
                        {
                            case "createddt":
                                auctions = from a in auctions
                                           orderby a.CreatedDb descending
                                           select a;
                                break;
                            case "price":
                                auctions = from a in auctions
                                           orderby a.Price descending
                                           select a;
                                break;
                            default:
                                auctions = from a in auctions
                                           orderby a.Id descending
                                           select a;
                                break;
                        }
                        break;
                    }
                default:
                    {
                        if (sort_key != null)
                            sort_key = sort_key.ToLower();

                        switch (sort_key)
                        {
                            case "createddt":
                                auctions = from a in auctions
                                           orderby a.CreatedDb
                                           select a;
                                break;
                            case "price":
                                auctions = from a in auctions
                                           orderby a.Price
                                           select a;
                                break;
                            default:
                                auctions = from a in auctions
                                           orderby a.Id
                                           select a;
                                break;
                        }
                        break;
                    }
            }

            return auctions.ToList();
        }

        private bool AuctionExists(int id)
        {
            return _context.Auctions.Any(e => e.Id == id);
        }
    }
}
