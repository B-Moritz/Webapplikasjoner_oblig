
using Webapplikasjoner_oblig.DAL;
using Webapplikasjoner_oblig.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using AlphaVantageInterface;
using AlphaVantageInterface.Models;
using Microsoft.EntityFrameworkCore;

namespace Webapplikasjoner_oblig.Controllers
{
    [Route("[controller]/[action]")]
    public class TradingController : ControllerBase
    {
        private readonly int _quoteCacheTime = 24;

        private readonly ITradingRepository _db;

        private readonly IConfiguration _config;

        private readonly ISearchResultRepositry _searchResultRepositry;

        private readonly string _apiKey;




        public TradingController(ITradingRepository db,ISearchResultRepositry searchResultRepositry, IConfiguration config)
        {
            _db = db;
            // Adding configuration object that contains the appsettings.json content
            _config = config;
            // We can now access the AlphaVantage api key:
            _apiKey = _config["AlphaVantageApi:ApiKey"];

            _searchResultRepositry = searchResultRepositry;
        }

        public async Task<Model.SearchResult> FindStock(string keyword) 
        {
            
           
                Model.SearchResult? searchResult = await _searchResultRepositry.GetOneKeyWordAsync(keyword);

                if(searchResult == null)
                {
                    return null;
                }

                return searchResult;
            
        }
        public async Task<bool> SaveSearchResult(string keyword)
        {
            try
            {
                var modelSearchResult = new Model.SearchResult();
                var res = _searchResultRepositry.GetOneKeyWordAsync(keyword);

                if (res is null)
                {
                    AlphaVantageConnection AlphaV = await AlphaVantageConnection.BuildAlphaVantageConnection(_apiKey, true);

                    var alphaObject = await AlphaV.findStockAsync(keyword);



                    modelSearchResult.SearchKeyword = keyword.ToUpper();
                    modelSearchResult.SearchTime = DateTime.Now;

                    var StockList = new List<Stock>();
                    foreach (var alphaStock in alphaObject.BestMatches)
                    {
                        var ApiStock = new Stock();
                        ApiStock = alphaStock;

                        StockList.Add(ApiStock);
                    }

                    var StockDetailsList = new List<StockDetail>();
                    foreach (var stock in StockList)
                    {
                        var stockDetails = new Model.StockDetail();
                        stockDetails.StockName = stock.Name;
                        stockDetails.StockSymbol = stock.Symbol;

                        StockDetailsList.Add(stockDetails);
                    }

                    modelSearchResult.StockList = StockDetailsList;

                    _searchResultRepositry.SaveSearchResultAsync(modelSearchResult);

                    return true;
                }
                else
                {

                    double timeSinceLastUpdate = (DateTime.Now - modelSearchResult.SearchTime).TotalHours;

                    if (timeSinceLastUpdate >= _quoteCacheTime)
                    {
                        _searchResultRepositry.DeleteSearchResult(modelSearchResult.SearchKeyword);

                        await _searchResultRepositry.SaveSearchResultAsync(modelSearchResult);

                        return true;
                    }

                    return false;

                }
                
            } catch(Exception e)
            {
                return false;
            }

            
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
            throw new NotImplementedException();
            /*// Create the api object
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
            if (curStockQuote is null || timeSinceLastUpdate > 2)
            {
                StockQuote newQuote = await AlphaV.getStockQuoteAsync(curStock.Symbol);
                await _db.AddStockQuoteAsync(newQuote, curStock);

            }
            // We now have a stock quote - find the total that needs to be added to the users funds
            // We need to handle currency



            // Call repository 
            // remove stock from user
            // Update total funds and other stats
            // Add Trade*/
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
