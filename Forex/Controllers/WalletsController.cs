using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forex.Models;
using Microsoft.AspNetCore.Authorization;

namespace Forex.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly UserContext _context;

        public WalletsController(UserContext context)
        {
            _context = context;
        }

        // GET: Wallets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wallet>>> GetWallets()
        {
            return await _context.Wallets.ToListAsync();
        }

        // GET: Wallets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wallet>> GetWallet(int id)
        {
            var wallet = await _context.Wallets.FindAsync(id);

            if (wallet == null)
            {
                return NotFound();
            }

            return wallet;
        }

        // GET: Wallets/User/5
        [HttpGet("User/{id}")]
        public async Task<ActionResult<Wallet>> GetUserWallet(int id)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(x => x.UserId == id);

            if (wallet == null)
            {
                return NotFound();
            }

            return wallet;
        }

        // PUT: Wallets/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWallet(int id, Wallet wallet)
        {
            if (id != wallet.WalletId)
            {
                return BadRequest();
            }

            _context.Entry(wallet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WalletExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool WalletExists(int id)
        {
            return _context.Wallets.Any(e => e.WalletId == id);
        }
    }
}
