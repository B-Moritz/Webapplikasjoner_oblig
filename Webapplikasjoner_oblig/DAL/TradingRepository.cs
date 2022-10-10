using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.Model;
using System.Linq;
using System.Reflection;

namespace Webapplikasjoner_oblig.DAL
{
    public class TradingRepository : ITradingRepository
    {
        private readonly TradingContext _db;

        public TradingRepository(TradingContext db)
        {
            _db = db;
        }

        //okab ... fra
        public async Task<List<Portfolio>> GetPortfolio(string symbol, DateTime startDate, DateTime endDate)
        {
            try
            {
                foreach(var history in _db.Stocks)
                {
                    var historic_data = await GetPortfolio(symbol, startDate, endDate);
                    var lastUpdated = System.IO.File.GetLastWriteTime(historic_data);
                    if(lastUpdated != history.LastUpdated)
                    {
                        history.LastUpdated = lastUpdated;
                    }

                    return historic_data;
                }
                //DbContext.saveChanges();
            }
            catch
            {
                return null;
            }
            return null;
        }
        // okab ...til

        public async Task<List<FavoriteList>> GetFavoriteList(int userId)
        {

          
            try
            {
                Users enUser = await _db.Users.FindAsync(userId);
                List<Stocks>? favorites = enUser.Favorites.ToList();
                
                return favorites;
                
            }
            catch
            {
                return null;
            }
        }
        // okab ...til








        public async Task<bool> SaveTradeAsync(Trade innTrading)
        {
            /**{
                try
                {
                    var nyTradingRad = new Trades()
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
            */
            throw new NotImplementedException();
        }



        public async Task<List<Trade>> GetAllTradesAsync()
        { /**
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
            */
            throw new NotImplementedException();
        }


        public async Task<Trade> GetOneTradeAsync(int id)
        {
            /**
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
            */
            throw new NotImplementedException();
        }
    }
}