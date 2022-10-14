
using Webapplikasjoner_oblig.DAL;
using Webapplikasjoner_oblig.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using AlphaVantageInterface;
using AlphaVantageInterface.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using EcbCurrencyInterface;

namespace Webapplikasjoner_oblig.Controllers
{
    [Route("[controller]/[action]")]
    public class TradingController : ControllerBase
    {
        private readonly ITradingRepository _db;

        private readonly IConfiguration _config;

        private readonly string _apiKey;

        private readonly int _quoteCacheTime = 24;

        public TradingController(ITradingRepository db, IConfiguration config)
        {
            _db = db;
            // Adding configuration object that contains the appsettings.json content
            _config = config;
            // We can now access the AlphaVantage api key:
            _apiKey = _config["AlphaVantageApi:ApiKey"];
        }

        public async Task<Model.SearchResult> FindStock(string keyword) 
        {
            // Denne metoden skal lage et søkeresultat 
            throw new NotImplementedException();
        }

       /* public async Task<Portfolio> GetPortfolio(int userId)
        {
            throw new NotImplementedException();   
        }*/

        // okab ... fra
        [HttpGet]
        public async Task<Portfolio> GetPortfolio(int userId)
        {
            return await _db.GetPortfolioAsync(userId);
        
        }

        /*public async Task<Portfolio> GetPortfolio(int userId)
        {
            try
            {
                foreach (var portfolio in _db.GetAllTradesAsync)
                {
                    var filename = portfolio.UsersId;
                    var lastChanged = System.IO.File.GetLastWriteTime(filename);

                    if (lastChanged != portfolio.LastChanged)
                    {
                        portfolio.LastChanged = lastChanged;
                    }

                }
                DbContext.saveChanges();
            }
            catch
            {
                var books = await _db.GetAllTradesAsync();
                return Ok(books);

            }
            // OKAB... TIL
        }*/

        public async Task<FavoriteList> GetFavoriteList(int userId)
        {
            return await _db.GetFavoriteList(userId);
        }
        

        public async Task<Portfolio> BuyStock(int userId, string symbol, int count)
        {
            throw new NotImplementedException();  
        }

        public async Task<Portfolio> SellStock(int userId, string symbol, int count)
        {
            // Test http request: localhost:1633/trading/sellStock?userId=1&symbol=MSFT&count=5
            // Check if the stock exists in the database
            Stocks curStock = _db.GetStock(symbol);
            if (curStock is null) 
            {
                throw new NullReferenceException("The stock was not found in the database");
            }
            // Get the updated quote for the stock
            StockQuotes curQuote = await getUpdatedQuote(symbol);

            // Get user
            User identifiedUser = await _db.GetUser(userId);

            // We now have a stock quote - find the total that needs to be added to the users funds
            // We need to handle currency
            decimal exchangeRate = 1;
            if (identifiedUser.Currency != curStock.Currency)
            {
                // Get the exchange rate from Ecb
                exchangeRate = await EcbCurrencyHandler.getExchangeRate(identifiedUser.Currency, curStock.Currency);
            }
            decimal saldo = (decimal) curQuote.Price * count * exchangeRate;

            // One transaction to sell the stocks
            await _db.SellStockTransactionAsync(userId, symbol, saldo, count);

            return await _db.GetPortfolioAsync(userId);
        }

        private async Task<StockQuotes> getUpdatedQuote(string symbol)
        {
            // Create the api object
            AlphaVantageConnection AlphaV = await AlphaVantageConnection.BuildAlphaVantageConnection(_apiKey, true);
            // Check if there are stock quotes
            StockQuotes curStockQuote = _db.GetStockQuote(symbol);
            if (curStockQuote is null)
            {
                // get a new quote from Alpha vantage api
                StockQuote newQuote = await AlphaV.getStockQuoteAsync(symbol);
                // Adding stock quote to db and get the StockQuotes object 
                StockQuotes newConvertedQuote = await _db.AddStockQuoteAsync(newQuote);
                // Set the new StockQuotes object as current quote
                curStockQuote = newConvertedQuote;
            }
            else
            {
                // Check that the stored quote is not outdated
                double timeSinceLastUpdate = (DateTime.Now - curStockQuote.Timestamp).TotalHours;

                if (timeSinceLastUpdate >= _quoteCacheTime)
                {
                    // If the quote was not updated within the specified _quoteCachedTime, then a new quote is obtained from api
                    // Remove the existing stock quotes from db
                    _db.RemoveStockQuotes(symbol);
                    StockQuote newQuote = await AlphaV.getStockQuoteAsync(symbol);
                    // Adding stock quote to db
                    StockQuotes newConvertedQuote = await _db.AddStockQuoteAsync(newQuote);
                    curStockQuote = newConvertedQuote;
                }
            }
            return curStockQuote;
        }

        public async Task<UserProfile> GetUserProfile(int userId)
        {
            // Denne metoden skal returnere informasjon om bruker, portfoliolisten og favorittlisten.
            // Egner seg når frontend lastes inn første gang.
            throw new NotImplementedException();
        }

        public async Task<bool> SaveTrade(Trade innTrading)
        {
            return await _db.SaveTradeAsync(innTrading);
        }

        public async Task<List<Trade>> GetAllTrades()
        {
            return await _db.GetAllTradesAsync();
        }

        public async Task<Trade> GetOneTrade(int id)
        {
            return await _db.GetOneTradeAsync(id);
        }

    }
}
