using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.Model;
using System.Linq;
using System.Reflection;
using System.Text;
using AlphaVantageInterface.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<Stocks> GetStockAsync(string symbol)
        {
            Stocks curStock = new Stocks();

            try
            {
                curStock = await _db.Stocks.SingleAsync(s => string.Compare(s.Symbol, symbol) == 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return curStock;
        }

        public async Task<StockQuotes> AddStockQuoteAsync(AlphaVantageInterface.Models.StockQuote stockQuote)
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


        public async Task<Users> GetPortfolioAsync(int userId)
        {
            // Get the user entity from database
            var user = await _db.Users.SingleAsync(u => u.UsersId == userId);
            return user;
            // Defining and initializing the list that will contain all stocks in the portfolio
            /*List<PortfolioStock> portfolio_list = new List<PortfolioStock>();

            Stocks stock;
            foreach (StockOwnerships min_stock in user.Portfolio)
            {
                stock = min_stock.Stock;
                
                // 



                // Creating the new portfolio stock object derived from the above calculations 
                var current_Portfolio_stock = new PortfolioStock
                {
                    Quantity = min_stock.StockCounter,
                    EstTotalMarketValue = 0,
                    TotalCost = min_stock.SpentValue,
                    Symbol = stock.Symbol,
                    StockName = stock.StockName,
                    Description = stock.Description,
                    StockCurrency = stock.Currency
                };

                portfolio_list.Add(current_Portfolio_stock);
            }

            var OutPortfolio = new Portfolio
            {
                LastUpdate = DateTime.Now,
                TotalValueSpent = 0,
                TotalPortfolioValue = 0,
                PortfolioCurrency = user.PortfolioCurrency,
                Stocks = portfolio_list
            };
            return OutPortfolio;*/
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

        public async Task AddToFavoriteListAsync(int userId, string symbol)
        {
            
            Users enUser = await _db.Users.SingleAsync(u => u.UsersId == userId);
            Stocks enStock = await _db.Stocks.SingleAsync(u => u.Symbol == symbol);           
            enUser.Favorites.Add(enStock);
            _db.SaveChanges();
        }
        public async Task DeleteFromFavoriteListAsync(int userId, string symbol)
        {

            Users enUser = await _db.Users.SingleAsync(u => u.UsersId == userId);
            Stocks enStock = await _db.Stocks.SingleAsync(u => u.Symbol == symbol);
            enUser.Favorites.Remove(enStock);
            _db.SaveChanges();

        }

        public async Task<User> GetUserAsync(int userId)
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

        public async Task BuyStockTransactionAsync(User curUser, Stocks curStock, decimal saldo, int count) {


            StockOwnerships curentOwnership = await _db.StockOwnerships.SingleAsync<StockOwnerships>(o => o.StocksId == curStock.Symbol && o.UsersId == curUser.Id);
            if (curentOwnership is null)
            {
                // The user has no existing ownership. Create new ownership
                StockOwnerships newOwnerhsip = new StockOwnerships
                {
                    StocksId = curStock.Symbol,
                    UsersId = curUser.Id,
                    StockCounter = count,
                    SpentValue = saldo,
                };

                _db.StockOwnerships.Add(newOwnerhsip);
            }
            else {
                // Add to the existing ownership
                curentOwnership.SpentValue += saldo;
                curentOwnership.StockCounter += count;
            }

            Users dbUser = await _db.Users.SingleAsync(u => u.UsersId == curUser.Id);
            dbUser.FundsAvailable -= saldo;

            // Add a trade object
            Trades newBuyTradeLog = new Trades
            {
                StockCount = count,
                TradeTime = DateTime.Now,
                UserIsBying = true,
                Saldo = saldo,
                Currency = dbUser.PortfolioCurrency,
                Stock = curStock,
                User = dbUser
            };
            await _db.Trades.AddAsync(newBuyTradeLog);

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



        public async Task<List<Trade>> GetAllTradesAsync(int userId)
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
            Users dbUser = await _db.Users.SingleAsync(u => u.UsersId == userId);
            List<Trades> curTrader = dbUser.Trades;
            List<Trade> alltrades = new List<Trade>();

            foreach(Trades curPortfolio in curTrader)
            {
                var newTrade = new Trade
                {
                    Id = curPortfolio.TradesId,
                    StockSymbol = curPortfolio.StocksId,
                    Date = curPortfolio.TradeTime,
                    UserId = curPortfolio.UsersId
                };
               
            }
            return alltrades;
            
            //throw new NotImplementedException();
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