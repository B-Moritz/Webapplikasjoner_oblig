using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public class TradingRepository : ITradingRepository
    {
        private readonly TradingContext _db;

        public TradingRepository(TradingContext db)
        {
            _db = db;
        }
        public async Task<bool> SaveTradeAsync(Trade innTrading)
        {
            try
            {
                var nyTradingRad = new Trade()
                {
                    Id = innTrading.Id,
                    StockSymbol = innTrading.StockSymbol,
                    Date = innTrading.Date,
                    UserId = innTrading.Id
                };

                _db.Trades.Add(nyTradingRad);
                await _db.SaveChangesAsync();
                return true;

            }
            catch
            {
                return false;
            }
        }



        public async Task<List<Trade>> GetAllTradesAsync()
        {
            try
            {
                List<Trade> allTrades = await _db.Trades.Select(k => new Trade
                {
                    Id = k.Id,
                    StockSymbol = k.StockSymbol,
                    Date = k.Date,
                    UserId = k.UserId

                }).ToListAsync();
                return allTrades;
            }
            catch
            {
                return null;
            }
        }


        public async Task<Trade> GetOneTradeAsync(int id)
        {
            Trade oneTrade = await _db.Trades.FindAsync(id);
            var hentetTrading = new Trade()
            {
                Id = oneTrade.Id,
                StockSymbol = oneTrade.StockSymbol,
                Date = oneTrade.Date,
                UserId = oneTrade.UserId

            };
            return hentetTrading;
        }

    }
}