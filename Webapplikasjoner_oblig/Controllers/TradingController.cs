
using Webapplikasjoner_oblig.DAL;
using Webapplikasjoner_oblig.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using AlphaVantageInterface;
using AlphaVantageInterface.Models;

namespace Webapplikasjoner_oblig.Controllers
{
    [Route("[controller]/[action]")]
    public class TradingController : ControllerBase
    {
        private readonly ITradingRepository _db;

        private readonly IConfiguration _config;

        private readonly string apiKey;

        public TradingController(ITradingRepository db, IConfiguration config)
        {
            _db = db;
            // Adding configuration object that contains the appsettings.json content
            _config = config;
            // We can now access the AlphaVantage api key:
            string apiKey = _config["AlphaVantageApi:ApiKey"];
        }

        public async Task<SearchResult> FindStock(string keyword) 
        {
            // Denne metoden skal lage et søkeresultat 
            throw new NotImplementedException();
        }

        public async Task<Portfolio> GetPortfolio(int userId)
        {
            throw new NotImplementedException();   
        }

        public async Task<FavoriteList> GetFavoriteList(int userId) 
        {
            throw new NotImplementedException();
        }

        public async Task<Portfolio> BuyStock(int userId, string symbol, int count)
        {
            throw new NotImplementedException();  
        }

        public async Task<Portfolio> SellStock(int userId, string symbol, int count)
        {
            // Create the api object
            AlphaVantageConnection AlphaV = await AlphaVantageConnection.BuildAlphaVantageConnection(apiKey, true);

            // Check if the stock exists in the database
            Stocks curStock = _db.GetStock(symbol);
            if (curStock is null) 
            {
                throw new NullReferenceException("The stock was not found in the database");
            }

            // Check if there are stock quotes
            StockQuotes curStockQuote = _db.GetStockQuote(symbol);
            double timeSinceLastUpdate = (DateTime.Now - curStockQuote.Timestamp).TotalHours;
            if (curStockQuote is null || timeSinceLastUpdate > 2);
            {
                StockQuote newQuote = await AlphaV.GetStockQuoteAsync(curStock.Symbol);
            }


            // Call repository 
            // remove stock from user
            // Update total funds and other stats
            // Add Trade
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
