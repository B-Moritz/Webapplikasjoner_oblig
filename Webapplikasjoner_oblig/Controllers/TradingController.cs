
using Webapplikasjoner_oblig.DAL;
using Webapplikasjoner_oblig.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Webapplikasjoner_oblig.Controllers
{
    [Route("[controller]/[action]")]
    public class TradingController : ControllerBase
    {
        private readonly ITradingRepository _db;

        private readonly IConfiguration _config;

        public TradingController(ITradingRepository db, IConfiguration config)
        {
            _db = db;
            // Adding configuration object that contains the appsettings.json content
            _config = config;
            // We can now access the AlphaVantage api key:
            // string apiKey = _config["AlphaVantageApi:ApiKey"];
        }


        public async Task<Portfolio> GetPortfolio(int userId, string symbol)
        {
            throw new NotImplementedException();    
        }

        public async Task<FavoriteList> GetFavoriteList(int userId) 
        {
            throw new NotImplementedException();
        }

        public async Task<> BuyStock(int userId, string symbol) 
        {
            
            throw new NotImplementedException();  
        }

        public async Task<> SellStock(int userId, string symbol)
        {
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
