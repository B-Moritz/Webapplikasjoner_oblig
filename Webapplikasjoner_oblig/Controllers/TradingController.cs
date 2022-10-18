
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

       



        public TradingController(ITradingRepository db,ISearchResultRepositry searchResultRepositry, IConfiguration config)
        {
            _db = db;
            // Adding configuration object that contains the appsettings.json content
            _config = config;
            // We can now access the AlphaVantage api key:
            _apiKey = _config["AlphaVantageApi:ApiKey"];
            // Accessing search results 
            _searchResultRepositry = searchResultRepositry;

        }

        /**
         * This method finds a search result that is stored in the database using a method from 
         * the search result repository. Null is returned if no match is found
         */
        public async Task<Model.SearchResult> FindStock(string keyword) 
        {
                Model.SearchResult? searchResult = await _searchResultRepositry.GetOneKeyWordAsync(keyword);

                if(searchResult is null)
                {
                    return null;
                }

                return searchResult;
        }


        /**
         * A function to add a search result using a received word from client 
         * and checking if record matching exixst in database. 
         * If it exist already it checks its last update, if last update is greater than 24 
         * then removes exixting record and add new one by creating a new object of search result and passing it to  saveSearchResult in repository.
         * If there is no such record, it fetches data from api using the keyword and save it by passing it to saveSearchResult function
         */
        public async Task<bool> SaveSearchResult(string keyword)
        {
            // Search result object from Model 
            var modelSearchResult = new Model.SearchResult();


            // Try and find a search result with given keyword in searchresults table 
            Model.SearchResult res = await _searchResultRepositry.GetOneKeyWordAsync(keyword);

            // If there is no such search result stored in the database, then go a head and fetch it from 
            // Alpha vantage api
            if (res is null)
            {
                // Connection to alpha vantage api
                AlphaVantageConnection AlphaV = await AlphaVantageConnection.BuildAlphaVantageConnection(_apiKey, true);


                // Fetch stocks from api using the given name 
                var alphaObject = await AlphaV.findStockAsync(keyword);
                    
                // Initiate a new searchResult object
                modelSearchResult.SearchKeyword = keyword.ToUpper();
                modelSearchResult.SearchTime = DateTime.Now;

                /*// List of stocks received from api call are set to be stock objects and added to a lsit
                var StockList = new List<Stock>();
                foreach (var alphaStock in alphaObject.BestMatches)
                {
                    var ApiStock = new Stock();
                    ApiStock = alphaStock;

                    StockList.Add(ApiStock);
                }*/

                // StockDetails are initilized by assigning properties from stocks. StockDetails are the added to a list
                var StockDetailsList = new List<StockDetail>();
                foreach (Stock stock in alphaObject.BestMatches)
                {
                    var stockDetails = new Model.StockDetail();
                    stockDetails.StockName = stock.Name;
                    stockDetails.StockSymbol = stock.Symbol;
                    stockDetails.Description = stock.Type;
                    stockDetails.Currency = stock.Currency;
                    stockDetails.LastUpdated = DateTime.Now;

                    StockDetailsList.Add(stockDetails);
                }

                // SearchResults list properties is assigned a list holding stockDetail objects.
                modelSearchResult.StockList = StockDetailsList;

                // SearchResult is passed to a function in searchResultRepositry to be added to the database
                await _searchResultRepositry.SaveSearchResultAsync(modelSearchResult);

                return true;
            }
            else
            {
                // If there exist a search result match then check when it was added.
                double timeSinceLastUpdate = (DateTime.Now - modelSearchResult.SearchTime).TotalHours;

                // if it has passed over 24 hours since it was added to table then remove it from table and add a new record
                if (timeSinceLastUpdate >= _quoteCacheTime)
                {
                    _searchResultRepositry.DeleteSearchResult(modelSearchResult.SearchKeyword);

                    await _searchResultRepositry.SaveSearchResultAsync(modelSearchResult);

                    return true;
                }

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

        public async Task<FavoriteList> DeleteFromFavoriteList(int userId, string symbol)
        {
            await _db.DeleteFromFavoriteListAsync(userId, symbol);

            return await GetFavoriteList(userId);
        }

        public async Task<FavoriteList> AddToFavoriteList(int userId, string symbol)
        {
            await _db.AddToFavoriteListAsync(userId, symbol);

            return await GetFavoriteList(userId);
        }

        public async Task<Portfolio> BuyStock(int userId, string symbol, int count)
        {

            // Test endpoint: localhost:1635/trading/buyStock?userId=1&symbol=MSFT&count=5
            // Validate count input value
            if (count < 1) {
                throw new ArgumentException("The provided count value is not valid. It must be an integer greater than 0.");
            }
            // Get the user object
            User curUser = await _db.GetUserAsync(userId);
            if (curUser is null)
            {
                throw new ArgumentException("The provided userId did not match any user in the database!");
            }
            // Get the stock
            Stocks curStock = await _db.GetStockAsync(symbol);
            if (curStock is null)
            {
                throw new ArgumentException("The specified stock was not found in the database");
            }
            // Calculate the saldo required to by the amount of stocks
            StockQuotes curQuote = await getUpdatedQuote(symbol);
            decimal exchangeRate = 1;
            if (curUser.Currency != curStock.Currency)
            {
                exchangeRate = await EcbCurrencyHandler.getExchangeRate(curStock.Currency, curUser.Currency);
            }
            decimal saldo = exchangeRate * (decimal)curQuote.Price * count;

            // Check that the user has the funds needed to perform the transaction
            if (curUser.FundsAvailable - saldo < 0)
            {
                throw new Exception("The user has not enough funds to perform this transaction!");
            }

            await _db.BuyStockTransactionAsync(curUser, curStock, saldo, count);

            return await GetPortfolio(userId);
        }

        public async Task<Portfolio> SellStock(int userId, string symbol, int count)
        {
            // Test http request: localhost:1633/trading/sellStock?userId=1&symbol=MSFT&count=5
            // Check if the stock exists in the database
            Stocks curStock = await _db.GetStockAsync(symbol);
            if (curStock is null) 
            {
                throw new NullReferenceException("The stock was not found in the database");
            }
            // Get the updated quote for the stock
            StockQuotes curQuote = await getUpdatedQuote(symbol);

            // Get user
            User identifiedUser = await _db.GetUserAsync(userId);

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
