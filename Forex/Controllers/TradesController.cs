using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forex.Models;

namespace Forex.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TradesController : ControllerBase
    {
        private readonly UserContext _context;

        public TradesController(UserContext context)
        {
            _context = context;
        }

        // GET: Trades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trade>>> GetTrades()
        {
            return await _context.Trades.ToListAsync();
        }

        // GET: Trades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trade>> GetTrade(int id)
        {
            var trade = await _context.Trades.FindAsync(id);

            if (trade == null)
            {
                return NotFound();
            }

            return trade;
        }

        // GET: Trades/User/5
        [HttpGet("User/{id}")]
        public async Task<ActionResult<IEnumerable<Trade>>> GetUserTrades(int id)
        {
            var trades = await _context.Trades.Include(y => y.BuyerOffer)
                .Include(y => y.SellerOffer)
                .Where(x => (x.BuyerOffer.UserId == id || x.SellerOffer.UserId == id))
                .ToListAsync();

            if (!trades.Any())
            {
                return NotFound();
            }

            return trades;
        }

        private bool TradeExists(int id)
        {
            return _context.Trades.Any(e => e.TradeId == id);
        }
    }
}
