﻿
using Webapplikasjoner_oblig.DAL;
using Webapplikasjoner_oblig.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.EntityFrameworkCore;

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

        public async Task<SearchResult> FindStock(string keyword) 
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
        public async Task<List<Portfolio>> GetPortfolioAsync(int userId)
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
            throw new NotImplementedException();
        }



        public async Task<Portfolio> BuyStock(int userId, string symbol, int count)
        {
            throw new NotImplementedException();  
        }

        public async Task<Portfolio> SellStock(int userId, string symbol, int count)
        {
            throw new NotImplementedException();
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
