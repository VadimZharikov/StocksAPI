using Forex.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Forex.Services
{
    public class BackgroundTrader : IHostedService, IDisposable
    {
        private readonly ILogger<BackgroundTrader> _logger;
        private Timer timer;
        public UserContext _context;
        private readonly IServiceScopeFactory scopeFactory;


        public BackgroundTrader(ILogger<BackgroundTrader> logger, IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            this._logger = logger;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(cb => Trade(),
                null,
                TimeSpan.FromSeconds(0),
                TimeSpan.FromSeconds(60)
                );
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service stopped");
            return Task.CompletedTask;
        }
        private async void Trade()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetRequiredService<UserContext>();
                _logger.LogInformation($"( {DateTime.Now}) Starting trading...");
                foreach (var company in _context.Offers
                    .Where(offer => offer.IsActive == true)
                    .GroupBy(offer => offer.StockId)
                    .Select(x => x.Key))
                {
                    var companyOffers = _context.Offers.Where(offer => offer.StockId == company);
                    await TradeOffers(companyOffers);
                }
                foreach (Item item in _context.Items)
                {
                    if (item.Quantity == 0)
                    {
                        _context.Items.Remove(item);
                    }
                }
                _context.SaveChanges();
                _logger.LogInformation("Trading stopped...");
            }

        }

        private Task TradeOffers(IQueryable<Offer> companyOffers)
        {
            if (companyOffers.Any(offer => offer.OfferType == "Sell"))
            {
                foreach (var sellOffer in companyOffers.Where(offer => offer.OfferType == "Sell").Where(offer => offer.IsActive == true).OrderBy(offer => offer.Price))
                {
                    if (companyOffers.Any(offer => offer.OfferType == "Buy"))
                    {
                        foreach (var buyOffer in companyOffers.Where(offer => offer.IsActive == true).Where(offer => offer.OfferType == "Buy"))
                        {
                            if (sellOffer.Price > buyOffer.Price)
                            {
                                continue;
                            }
                            if(buyOffer.StocksLeft <= 0)
                            {
                                buyOffer.IsActive = false;
                                break;
                            }
                            if (sellOffer.StocksLeft <= 0)
                            {
                                sellOffer.IsActive = false;
                                continue;
                            }
                            Transact(sellOffer, buyOffer);
                        }
                        continue;
                    }
                    break;
                }
            }
            return Task.CompletedTask;
        }

        private Task Transact(Offer sellOffer, Offer buyOffer)
        {
            /*using (var transaction = _context.Database.BeginTransaction())
            {*/
                try
                {
                    var buyer = _context.Users.Include(user => user.Wallet).Include(user => user.Items).Single(user => user.UserId == buyOffer.UserId && user.Wallet.Funds > 0);
                    var seller = _context.Users.Include(user => user.Wallet).Include(user => user.Items).Single(user => user.UserId == sellOffer.UserId);

                    if (buyer == null || seller == null)
                    {
                        throw new ArgumentNullException();
                    }
                    if (buyer.UserId == seller.UserId)
                    {
                        //transaction.Rollback();
                        return Task.CompletedTask;
                    }
                    int quantCanBuy = Math.Min(Convert.ToInt32(buyer.Wallet.Funds / sellOffer.Price), buyOffer.StocksLeft);
                    var quantToSell = Math.Min(sellOffer.StocksLeft, quantCanBuy);
                    var quantCanSell = Math.Min(seller.Items.Single(item => item.StockId == buyOffer.StockId).Quantity, quantToSell);
                    var totalPrice = quantCanSell * sellOffer.Price;

                    Purchase(sellOffer, buyOffer, buyer, seller, totalPrice, quantCanSell);
                    //transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Transaction failed: {ex.Message}");
                    //transaction.Rollback();
                }
            //}
            return Task.CompletedTask;
        }

        private void Purchase(Offer sellOffer, Offer buyOffer, User buyer, User seller, decimal totalPrice, int quantCanSell)
        {
            if (buyer.Wallet.Funds >= totalPrice && seller.Items
                .Any(item => item.StockId == buyOffer.StockId && item.Quantity >= buyOffer.StocksLeft))
            {
                sellOffer.StocksLeft -= quantCanSell;
                buyOffer.StocksLeft -= quantCanSell;
                buyer.Wallet.Funds -= totalPrice;
                seller.Wallet.Funds += totalPrice;
                if (buyer.Items.Any(item => item.StockId == sellOffer.StockId))
                {
                    buyer.Items.Single(item => item.StockId == sellOffer.StockId).Quantity += quantCanSell;
                }
                else
                {
                    buyer.Items.Add(new Item() { 
                    Quantity = quantCanSell,
                    StockId = sellOffer.StockId,
                    UserId = buyOffer.UserId
                    });
                }
                seller.Items.Single(item => item.StockId == buyOffer.StockId).Quantity -= quantCanSell;

                if (sellOffer.StocksLeft == 0)
                {
                    sellOffer.IsActive = false;
                }
                if (buyOffer.StocksLeft == 0)
                {
                    buyOffer.IsActive = false;
                }

                var trade = new Trade()
                {
                    Seller = seller.UserName,
                    Buyer = buyer.UserName,
                    Quantity = quantCanSell,
                    TotalPrice = totalPrice,
                    SellerOfferId = sellOffer.OfferId,
                    BuyerOfferId = buyOffer.OfferId,
                    BuyerOffer = buyOffer,
                    SellerOffer = sellOffer

                };
                _context.Add(trade);
                _logger.LogInformation($"{seller.UserName} sold {quantCanSell} for {totalPrice} and got {sellOffer.StocksLeft} stocks left to sell, ({buyOffer.StocksLeft}) buyer wants to buy");
            }
        }
    }
}
