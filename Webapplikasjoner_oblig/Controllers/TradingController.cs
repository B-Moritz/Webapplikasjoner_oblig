
using Webapplikasjoner_oblig.DAL;
using Webapplikasjoner_oblig.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using AlphaVantageInterface;
using AlphaVantageInterface.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using EcbCurrencyInterface;
using System.Diagnostics;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

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



       

        

        private readonly SearchResultRepositry SearchResultRepositry;



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

        [HttpGet]
        public async Task<Portfolio> GetPortfolio(int userId)
        {
            Portfolio outPortfolio =  await _db.GetPortfolioAsync(userId);
            // Add the total funds spent value:
            // Get the quote:
            StockQuotes curQuote;         
            decimal PortfolioTotalValue = 0;
            decimal totalValueSpent = 0;
            decimal exchangeRate = 1;
            foreach (PortfolioStock stock in outPortfolio.Stocks)
            {
                curQuote = await getUpdatedQuote(stock.Symbol);
                // Check the currency
                if (outPortfolio.PortfolioCurrency != stock.StockCurrency)
                {
                    exchangeRate = await EcbCurrencyHandler.getExchangeRate(stock.StockCurrency, outPortfolio.PortfolioCurrency);
                }
                stock.TotalValue = (decimal) curQuote.Price * stock.StockCounter * exchangeRate;
                PortfolioTotalValue += stock.TotalValue;
                totalValueSpent += stock.TotalFundsSpent;
            }
            outPortfolio.TotalPortfolioValue = PortfolioTotalValue;
            outPortfolio.TotalValueSpent = totalValueSpent;
            return outPortfolio;        
        }

        public async Task<FavoriteList> GetFavoriteList(int userId)
        {
            return await _db.GetFavoriteList(userId);
        }
        

        public async Task<Portfolio> BuyStock(int userId, string symbol, int count)
        {
            return await GetPortfolio(userId);

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
                exchangeRate = await EcbCurrencyHandler.getExchangeRate(curStock.Currency, identifiedUser.Currency);
                // Likning
                // n curStock_cur = exchange * n user_cur
            }
            // Calculating the total saldo with the amount of stocks and correct currency
            decimal saldo = (decimal) curQuote.Price * count * exchangeRate;

            Debug.WriteLine($"\n*******\nStock Price: {curQuote.Price}\nStock Currency: {curStock.Currency}\n" +
                              $"User currency: {identifiedUser.Currency}\nResult: {saldo}\n*******\n");

            // One transaction to sell the stocks
            await _db.SellStockTransactionAsync(userId, symbol, saldo, count);

            return await GetPortfolio(userId);
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

        public async Task ClearTradeHistory(int userId)
        {
            throw new NotImplementedException();
        }

    }
}
