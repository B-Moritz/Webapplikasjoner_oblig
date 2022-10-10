using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.Model;
using System.Linq;

using System.Reflection;

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


        public async Task<Portfolio> GetPortfolioAsync(int userId)
        {
            try
            {
                var user = _db.Users.Single(u => u.UsersId == userId);
                //user.Portfolio.<StockOwnerships>(s => s.UsersId, use)
                List<StockOwnerships> stockList = user.Portfolio;
                List<PortfolioStock> portfolio_list = new List<PortfolioStock>();


                foreach (var min_stock in stockList)
                {
                    // var stock_name = min_stock.UsersId;
                    var stock = min_stock.Stock;

                    var current_Portfolio_stock = new PortfolioStock
                    {
                        StockCounter = min_stock.StockCounter,
                        TotalValue = 0,
                        TotalFundsSpent = 0,
                        Symbol = stock.Symbol,
                        StockName = stock.StockName,
                        Description = stock.Description

                    };

                    portfolio_list.Add(current_Portfolio_stock);

                    //var s = DateTime.Now.Date.AddYears(stock_name);
                   /* var minPortfolioValue = await GetPortfolioAsync(stock_name);


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

                var OutPortfolio = new Portfolio
                {
                    LastUpdate = DateTime.Now,
                    TotalValueSpent = 0,
                    TotalPortfolioValue = 0,
                    Stocks = portfolio_list
                };
                return OutPortfolio;

            }
            catch
            {
                return null;
            }
            return null;        
        }


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