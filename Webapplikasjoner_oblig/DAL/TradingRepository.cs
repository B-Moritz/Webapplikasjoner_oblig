using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.Model;
using System.Linq;
using System.Reflection;
using System.Text;
using AlphaVantageInterface.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using PeanutButter.Utils;
using System.Diagnostics;
using EcbCurrencyInterface;

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

        public async Task<Stocks>? GetStockAsync(string symbol)
        {
            Stocks? curStock = await _db.Stocks.FindAsync(symbol);

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
            Stocks curStock = await _db.Stocks.SingleAsync<Stocks>(s => s.Symbol == stockQuote.Symbol);
            curStock.StockQuotes.Add(newTableRow);
            _db.SaveChanges();

            return newTableRow;
        }


        public async Task<Users> GetUsersAsync(int userId)
        {
            // Get the user entity from database
            var user = await _db.Users.SingleAsync(u => u.UsersId == userId);
            return user;
        }

        
        public async Task<FavoriteList> GetFavoriteList(int userId)
        { 
            try
            {
                Users enUser = await _db.Users.SingleAsync(u => u.UsersId == userId);
                List<Stocks>? favorites = enUser.Favorites;
                List<StockBase>? stockFavorite = new List<StockBase>();
                StockBase currentStockDetail;

                foreach (Stocks currentStock in favorites)
                {
                    currentStockDetail = new StockBase
                    {
                        StockName = currentStock.StockName,
                        Symbol = currentStock.Symbol,
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
                FundsSpent = string.Format("{0:N} {1}",curUser.FundsSpent, curUser.PortfolioCurrency),
                FundsAvailable = string.Format("{0:N} {1}",curUser.FundsAvailable, curUser.PortfolioCurrency),
                Currency = curUser.PortfolioCurrency
            };
            return convertedUser;
        }

        public async Task UpdateUserAsync(User curUser) {
            Users oldUser = await _db.Users.SingleAsync<Users>(u => u.UsersId == curUser.Id);
            if (oldUser is null) {
                throw new Exception("No such user in db!");
            }

            oldUser.FirstName = curUser.FirstName;
            oldUser.LastName = curUser.LastName;
            oldUser.Email = curUser.Email;

            // Change currency
            decimal exchangeRate = 1;
            if (oldUser.PortfolioCurrency != curUser.Currency)
            {
                exchangeRate = await EcbCurrencyHandler.getExchangeRateAsync(oldUser.PortfolioCurrency, curUser.Currency);
            }
            oldUser.FundsAvailable = oldUser.FundsAvailable * exchangeRate;
            oldUser.FundsSpent = oldUser.FundsSpent * exchangeRate;
            oldUser.PortfolioCurrency = curUser.Currency;
            foreach (StockOwnerships ownership in oldUser.Portfolio)
            {
                ownership.SpentValue = ownership.SpentValue * exchangeRate;
            }
            _db.SaveChanges();
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
            if (curOwnership.StockCounter < count) {
                throw new ArgumentException("The specified amount of stocks to sell exceeds the amount of shares owned!");
            }
            curOwnership.StockCounter -= count;
            curOwnership.SpentValue -= saldo;

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

            _db.Trades.Add(tradeLog);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task BuyStockTransactionAsync(Users curUser, Stocks curStock, decimal saldo, int count) {
            StockOwnerships currentOwnership;
            try
            {
                currentOwnership = _db.StockOwnerships.Single<StockOwnerships>(o => o.StocksId == curStock.Symbol && o.UsersId == curUser.UsersId);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex);
                currentOwnership = null;
            }
            
            if (currentOwnership is null)
            {
                // The user has no existing ownership. Create new ownership
                StockOwnerships newOwnership = new StockOwnerships
                {
                    StocksId = curStock.Symbol,
                    UsersId = curUser.UsersId,
                    StockCounter = count,
                    SpentValue = saldo,
                };

                _db.StockOwnerships.Add(newOwnership);
            }
            else {
                // Add to the existing ownership
                currentOwnership.SpentValue += saldo;
                currentOwnership.StockCounter += count;
            }

            Users dbUser = _db.Users.Single(u => u.UsersId == curUser.UsersId);
            dbUser.FundsAvailable -= saldo;
            dbUser.FundsSpent += saldo;

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
            dbUser.Trades.Add(newBuyTradeLog);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex) 
            { 
                Debug.WriteLine(ex);
            }
            
        }

        public async Task<List<Trade>> GetAllTradesAsync(int userId)
        {
            Users dbUser = await _db.Users.SingleAsync(u => u.UsersId == userId);
            List<Trades> curTrader = dbUser.Trades;
            List<Trade> trasaksjons = new List<Trade>();

            foreach(Trades curPortfolio in curTrader)
            {
                var newTrade = new Trade
                {
                    Id = curPortfolio.TradesId,
                    StockSymbol = curPortfolio.StocksId,
                    Date = curPortfolio.TradeTime,
                    UserId = curPortfolio.UsersId,
                    TransactionType = (curPortfolio.UserIsBying ? "Buying" : "Selling"),
                    StockCount = curPortfolio.StockCount,
                    Saldo = string.Format("{0:N} {1}", curPortfolio.Saldo, dbUser.PortfolioCurrency)
                };
                trasaksjons.Add(newTrade);
            }
            return trasaksjons;
        }

        public async Task ClearAllTradeHistoryAsync(int userId)
        {
            Users enUser = await _db.Users.SingleAsync(u => u.UsersId == userId);
            enUser.Trades.Clear();

            _db.SaveChanges();

        }
        public async Task<User> ResetPortfolio(int userId) {
            // Obtain the user entity
            Users curUser = await _db.Users.SingleAsync<Users>(u => u.UsersId == userId);
            // Remove trade history of user
            curUser.Trades.Clear();
            curUser.Favorites.Clear();
            curUser.Portfolio.Clear();
            curUser.FundsAvailable = 1000000M;
            curUser.FundsSpent = 0;
            curUser.PortfolioCurrency = "NOK";

            _db.SaveChanges();

            User outObj = new User
            {
                Id = curUser.UsersId,
                FirstName = curUser.FirstName,
                LastName = curUser.LastName,
                Email = curUser.Email,
                FundsAvailable = string.Format("{0:N} {1}", curUser.FundsAvailable, curUser.PortfolioCurrency),
                FundsSpent = string.Format("{0:N} {1}", curUser.FundsSpent, curUser.PortfolioCurrency),
                Currency = curUser.PortfolioCurrency
            };
            return outObj;
        }
        public async Task CleanTradingSchemaAsync() 
        {
            // This method will remove stocks, stock quotes and search results, which are older than a day

            // Get the old search results
            List<SearchResults> searchResults = await _db.SearchResults.ToListAsync();
            IEnumerable<SearchResults> oldResults = searchResults.Where(searchResult => (DateTime.Now - searchResult.SearchTimestamp).TotalHours >= 24);
            _db.SearchResults.RemoveRange(oldResults);
            // Remove all stocks that have no reference to a user or a search result
            var oldStocks = await _db.Stocks.Where(curStock => (curStock.SearchResults.Count() == 0) && (curStock.FavoriteUsers.Count() == 0)).ToListAsync();
            foreach (Stocks oldStock in oldStocks)
            {
                if (oldStock.StockQuotes is not null)
                {
                    _db.StockQuotes.RemoveRange(oldStock.StockQuotes);
                }
                _db.Stocks.Remove(oldStock);
            }
            await _db.SaveChangesAsync();
        }
    }
}