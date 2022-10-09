using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.Model;
using System.Linq;
using System.Text;

namespace Webapplikasjoner_oblig.DAL
{
    public class TradingRepository : ITradingRepository
    {
        private readonly TradingContext _db;

        public TradingRepository(TradingContext db)
        {
            _db = db;
        }


        public async Task<List<Portfolio>> GetPortfolioAsync(int userId)
        {
            try
            {
                foreach (var min_stock in _db.Trades)
                {
                    var stock_name = min_stock.UsersId;
                    //var s = DateTime.Now.Date.AddYears(stock_name);
                    var minPortfolioValue = await GetPortfolioAsync(stock_name);

                    if (minPortfolioValue != min_stock.minPortfolioValue)
                    {
                        min_stock.minPortfolioValue = minPortfolioValue;
                        //return minPortfolioValue;
                    }               
                    List<Portfolio> my_portfolio = await _db.Portfolio.Select(userId => new Portfolio
                    {
                        LastUpdate = userId.LastUpdate,
                        TotalValueSpent = userId.TotalValueSpent,
                        TotalPortfolioValue = userId.TotalPortfolioValue,
                        Stocks = userId.Stocks
                    }).ToListAsync();
                    return my_portfolio;

                    //return minPortfolioValue;
                    

                    /* Portfolio enPortfolioValue = await _db.Portfolio.FindAsync(userId);
                     var portfolioValue = new Portfolio()
                     {

                         LastUpdate = enPortfolioValue.LastUpdate,
                         TotalValueSpent = enPortfolioValue.TotalValueSpent,
                         TotalPortfolioValue = enPortfolioValue.TotalPortfolioValue,
                         Stocks = enPortfolioValue.Stocks                    

                     };*/
                }
            }
            catch
            {
                return null;
            }
            return null;        
        }

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