using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
                    StockName = innTrading.StockName,
                    Date = innTrading.Date,
                    User = innTrading.User
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
                    StockName = k.StockName,
                    Date = k.Date,
                    User = k.User

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
                StockName = oneTrade.StockName,
                Date = oneTrade.Date,
                User = oneTrade.User

            };
            return hentetTrading;
        }

    }
}