using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.Model;
using System.Linq;
using System.Reflection;
using System.Text;
using AlphaVantageInterface.Models;
using System.Text.RegularExpressions;
using PeanutButter.Utils;

namespace Webapplikasjoner_oblig.DAL
{
    public class TradingRepository : ITradingRepository
    {
        private readonly TradingContext _db;

        public TradingRepository(TradingContext db)
        {
            _db = db;
        }

        public void RemoveStockQuotes(string symbol)
        {
            // This method removes all quotes of the specified stock. There should only be one quote stored for each stock
            var removeQuotes = _db.StockQuotes.Where<StockQuotes>(t => t.StocksId == symbol);
            _db.StockQuotes.RemoveRange(removeQuotes);
        }

        public StockQuotes? GetStockQuote(string symbol)
        {
            StockQuotes? stockQuote = null;
            // Check if the stock quote exists
            try
            {
                stockQuote = _db.StockQuotes?.Single(sq => string.Compare(sq.StocksId, symbol) == 0);
            }
            catch (Exception e)
            { 
                Console.WriteLine(e.ToString());
            }

            return stockQuote;
        }

        public Stocks GetStock(string symbol)
        {
            Stocks curStock = new Stocks();

            try
            {
                curStock = _db.Stocks.Single<Stocks>(s => string.Compare(s.Symbol, symbol) == 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return curStock;
        }

        public async Task<StockQuotes> AddStockQuoteAsync(StockQuote stockQuote)
        {
            // This method converts an AlphaVantageInterface.Model.StockQuote object to
            // DAL.StockQuotes.
            // Then the new quote is added to the database.

            // Parse the LatestTradingDay to datetime object
            Regex LatestTradingdayPattern = new Regex("([0-9]*)-([0-9]*)-([0-9]*)");
            Match matches = LatestTradingdayPattern.Match(stockQuote.LatestTradingDay);
            GroupCollection gc = matches.Groups;

            StockQuotes newTableRow = new StockQuotes
            {
                StocksId = stockQuote.Symbol,
                Timestamp = DateTime.Now,
                LatestTradingDay = new DateTime(
                                                int.Parse(gc[1].ToString()),
                                                int.Parse(gc[2].ToString()),
                                                int.Parse(gc[3].ToString())
                                                ),
                Open = stockQuote.Open,
                Low = stockQuote.Low,
                High = stockQuote.High,
                Price = stockQuote.Price,
                Volume = stockQuote.Volume,
                PreviousClose = stockQuote.PreviousClose,
                Change = stockQuote.Change,
                ChangePercent = stockQuote.ChangePercent,
            };

            // Add the new object to the database (asuming that stock already is in database)
            await _db.StockQuotes.AddAsync(newTableRow);
            _db.SaveChanges();

            return newTableRow;
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

        
        public async Task<FavoriteList> GetFavoriteList(int userId)
        { 
            try
            {
                Users enUser = await _db.Users.SingleAsync(u => u.UsersId == userId);
                List<Stocks>? favorites = enUser.Favorites;
                List<StockDetail>? stockFavorite = new List<StockDetail>();
                StockDetail currentStockDetail;

                foreach (Stocks currentStock in favorites)
                {
                    currentStockDetail = new StockDetail
                    {
                        StockName = currentStock.StockName,
                        StockSymbol = currentStock.Symbol,
                        Description = currentStock.Description,
                        LastUpdated = currentStock.LastUpdated
                    };
                    stockFavorite.Add(currentStockDetail);
                }
                var currentFavorite = new FavoriteList
                {
                    LastUpdated = DateTime.Now,
                    StockList = stockFavorite
                };
                return currentFavorite;
            }
            catch
            {
                return null;
            }   
            
        }

        public async Task<User> GetUser(int userId)
        {
            // This method return the user object containing information about the user
            Users curUser = _db.Users.Single(t => t.UsersId == userId);
            
            User convertedUser = new User
            {
                Id = curUser.UsersId,
                FirstName = curUser.FirstName,
                LastName = curUser.LastName,
                Email = curUser.Email,
                Password = curUser.Password,
                FundsSpent = curUser.FundsSpent,
                FundsAvailable = curUser.FundsAvailable,
                Currency = curUser.PortfolioCurrency
            };
            return convertedUser;
        }

        public async Task SellStockTransactionAsync(int userId, string symbol, decimal saldo, int count) 
        {
            // This method adds to the total funds available
            // Removes the stocks
            // Adds a trade object
            Users curUser = _db.Users.Single<Users>(t => t.UsersId == userId);
            curUser.FundsAvailable += saldo;

            // Remove stock 
            StockOwnerships curOwnership = curUser.Portfolio.Single<StockOwnerships>(t => t.StocksId == symbol);
            curOwnership.StockCounter -= count;
            if (curOwnership.StockCounter <= 0)
            {
                // The user has no more ownership of this stock type
                curUser.Portfolio.Remove(curOwnership);
            }

            // Create new trade object
            Trades tradeLog = new Trades {
                StockCount = count,
                TradeTime = DateTime.Now,
                UserIsBying = false,
                Saldo = saldo,
                Currency = curUser.PortfolioCurrency,
                Stock = curOwnership.Stock,
                User = curOwnership.User
            };

            await _db.Trades.AddAsync(tradeLog);

            _db.SaveChanges();

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