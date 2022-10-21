
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
using System.Diagnostics.Metrics;

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
                    var SavedResult = SaveSearchResult(keyword);
                     
                     if (SavedResult is null)
                  {
                       return null;
                  }

                      return await SavedResult;
                }

                return searchResult;
        }

        public async Task<List<Model.SearchResult>> GetAllFromDB()
        {
            var list = _searchResultRepositry.GetAllKeyWordsAsync();

            if(list is null)
            {
                return null;
            }

            return await list;
        }


        /**
         * A function to add a search result using a received word from client 
         * and checking if record matching exixst in database. 
         * If it exist already it checks its last update, if last update is greater than 24 
         * then removes exixting record and add new one by creating a new object of search result and passing it to  saveSearchResult in repository.
         * If there is no such record, it fetches data from api using the keyword and save it by passing it to saveSearchResult function
         */
        public async Task<Model.SearchResult> SaveSearchResult(string keyword)
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

                return modelSearchResult;
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

                    return modelSearchResult;
                }

                return null;

            }
                
        }

        public async Task<Portfolio> GetPortfolio(int userId)
        {
            // Obtain the user from the database
            Users curUser = await _db.GetPortfolioAsync(userId);

            // Create the Portfolio object that the endpoint should return
            Portfolio outPortfolio = new Portfolio();
            outPortfolio.Stocks = new List<PortfolioStock>();

            // Defining variables
            StockQuotes curQuote;
            Stocks curStock;
            decimal portfolioTotalValue = 0;
            decimal totalValueSpent = 0;
            decimal exchangeRate = 1;
            decimal curStockValue = 0;
            decimal curStockPrice = 0;
            PortfolioStock newPortfolioStock;
            string userCurrency = curUser.PortfolioCurrency;
            decimal curUnrealizedPL = 0;
            List<decimal> totalStockValueList = new List<decimal>();

            foreach (StockOwnerships ownership in curUser.Portfolio)
            {
                newPortfolioStock = new PortfolioStock();
                curQuote = await GetUpdatedQuote(ownership.StocksId);
                curStock = ownership.Stock;

                // Add the symbol
                newPortfolioStock.Symbol = curStock.Symbol;
                // Add stock name
                newPortfolioStock.StockName = curStock.StockName;
                // Add description
                newPortfolioStock.Description = curStock.Description;
                // Add stock currency
                newPortfolioStock.StockCurrency = curStock.Currency;
                // Add the qunatity to the out object
                newPortfolioStock.Quantity = ownership.StockCounter;

                // Check the currency
                if (userCurrency != curStock.Currency)
                {
                    exchangeRate = await EcbCurrencyHandler.getExchangeRate(curStock.Currency, userCurrency);
                }
                // Add the estimated price obtained from the quote
                curStockPrice = exchangeRate * (decimal) curQuote.Price;
                newPortfolioStock.EstPrice = String.Format("{0:N} {1}", curStockPrice, userCurrency);

                // Calculate the estimated market value of the shares owned by the user
                curStockValue = (decimal) curQuote.Price * ownership.StockCounter * exchangeRate;
                newPortfolioStock.EstTotalMarketValue = String.Format("{0:N} {1}", curStockValue, userCurrency);
                // Add the estimated stock value to the total portfolio value
                portfolioTotalValue += curStockValue;
                totalStockValueList.Add(curStockValue);

                // Set the total cost
                newPortfolioStock.TotalCost = String.Format("{0:N} {1}", ownership.SpentValue, userCurrency); 
                totalValueSpent += ownership.SpentValue;

                // Find the unrealized profit/loss
                curUnrealizedPL = curStockValue - ownership.SpentValue;
                newPortfolioStock.UnrealizedPL = String.Format("{0}{1:N} {2}", 
                                                              (curUnrealizedPL > 0 ? "+" : ""), 
                                                               curUnrealizedPL,
                                                               userCurrency);
                outPortfolio.Stocks.Add(newPortfolioStock);
            }

            for (int i = 0; i < totalStockValueList.Count; i++) {
                // Finde the relative size of the stock compared to the entire portfolio
                outPortfolio.Stocks[i].PortfolioPortion = String.Format("{0:N}%", (totalStockValueList[i] / portfolioTotalValue) * 100);
            }

            outPortfolio.EstPortfolioValue = String.Format("{0:N} {1}", portfolioTotalValue, userCurrency);
            outPortfolio.TotalValueSpent = String.Format("{0:N} {1}",totalValueSpent, userCurrency);
            outPortfolio.BuyingPower = String.Format("{0:N} {1}", curUser.FundsAvailable, userCurrency);
            decimal unrealizedPortfolioPL = portfolioTotalValue - totalValueSpent;
            outPortfolio.UnrealizedPL = String.Format("{0}{1:N} {2}",
                                                     (unrealizedPortfolioPL > 0 ? "+" : ""),
                                                      unrealizedPortfolioPL,
                                                      userCurrency);
            outPortfolio.PortfolioCurrency = userCurrency;
            outPortfolio.LastUpdate = DateTime.Now;
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
            StockQuotes curQuote = await GetUpdatedQuote(symbol);
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
            // Validate stock counter. It must be an positive integer greather than or equal to 1
            if (count < 1) {
                throw new ArgumentException("The provided count value is invalid");
            }

            // Get the updated quote for the stock
            StockQuotes curQuote = await GetUpdatedQuote(symbol);

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

        private async Task<StockQuotes> GetUpdatedQuote(string symbol)
        {
            // Create the api object
            AlphaVantageConnection AlphaV = await AlphaVantageConnection.BuildAlphaVantageConnection(_apiKey, true);
            // Check if there are stock quotes
            StockQuotes curStockQuote = _db.GetStockQuote(symbol);
            if (curStockQuote is null)
            {
                // get a new quote from Alpha vantage api
                AlphaVantageInterface.Models.StockQuote newQuote = await AlphaV.getStockQuoteAsync(symbol);
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
                    AlphaVantageInterface.Models.StockQuote newQuote = await AlphaV.getStockQuoteAsync(symbol);
                    // Adding stock quote to db
                    StockQuotes newConvertedQuote = await _db.AddStockQuoteAsync(newQuote);
                    curStockQuote = newConvertedQuote;
                }
            }
            return curStockQuote;
        }

        public async Task<Model.StockQuote> GetStockQuote(string symbol) {
            StockQuotes curQuote = await GetUpdatedQuote(symbol);
            string stockCurrency = curQuote.Stock.Currency;
            Model.StockQuote newStockQuote = new Model.StockQuote {
                Symbol = curQuote.StocksId,
                StockName = curQuote.Stock.StockName,
                LastUpdated = curQuote.Timestamp,
                Open = String.Format("{0:N} {1}", curQuote.Open, stockCurrency),
                High = String.Format("{0:N} {1}", curQuote.High, stockCurrency),
                Low = String.Format("{0:N} {1}", curQuote.Low, stockCurrency),
                Price = String.Format("{0:N} {1}", curQuote.Price, stockCurrency),
                Volume = curQuote.Volume,
                LatestTradingDay = curQuote.LatestTradingDay,
                PreviousClose = String.Format("{0:N} {1}", curQuote.PreviousClose, stockCurrency),
                Change = curQuote.Change.ToString(),
                ChangePercent = curQuote.ChangePercent + " %"
            };
            return newStockQuote;
        }

        public async Task<bool> SaveTrade(Trade innTrading)
        {
            return await _db.SaveTradeAsync(innTrading);
        }

        public async Task<List<Trade>> GetAllTrades(int userId)
        {

            return await _db.GetAllTradesAsync(userId);
        }

        public async Task<Trade> GetOneTrade(int id)
        {
            return await _db.GetOneTradeAsync(id);
        }

        public async Task<Portfolio> ClearTradeHistory(int userId, string symbol)
        {
            await _db.ClearTradeHistoryAsync(userId, symbol);

            return await ClearTradeHistory(userId);

        }

        private Task<Portfolio> ClearTradeHistory(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
