using Webapplikasjoner_oblig.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PeanutButter.Utils;
using System.Diagnostics;

namespace Webapplikasjoner_oblig.DAL
{
    public class SearchResultRepositry : ISearchResultRepositry
    {
        private readonly TradingContext _db;

        public SearchResultRepositry(TradingContext db)
        {
            _db = db;
        }

        
        public async Task<List<SearchResult>> GetAllKeyWordsAsync()
        {   
          
                //list of all search results
                List<SearchResults> keywords = await _db.SearchResults.ToListAsync();

            if (!(keywords is null))
            {
                //list to hold search result objects
                var result = new List<SearchResult>();

                //list to replace the list of stock retrived from searchResults table with stockDetails objects
                var stockDList = new List<StockDetail>();

                //search result object to be used when mapping from searchresults table
                var searchRusltObject = new SearchResult();

                //looping over anonymous type retived from database
                foreach (var keyword in keywords)
                {
                    //looping over the list in keywords 
                    foreach (var stock in keyword.Stocks)
                    {
                        //mapping stock to stockDetails
                        var stockDetail = new StockDetail()
                        {
                            StockSymbol = stock.Symbol,
                            StockName = stock.StockName,
                            Description = stock.Description,
                            Currency = stock.Currency,
                            LastUpdated = stock.LastUpdated,
                        };

                        stockDList.Add(stockDetail);
                    }

                    //mapping searchResults element to searchResult object
                    searchRusltObject.SearchKeyword = keyword.SearchKeyword;
                    searchRusltObject.SearchTime = keyword.SearchTimestamp;
                    searchRusltObject.StockList = stockDList;

                    result.Add(searchRusltObject);
                }
                return result;
            }

            return null;
        }


        public async Task<SearchResult>? GetOneKeyWordAsync(string keyWord)
        {
            if (keyWord == null)
            {
                throw new ArgumentNullException();
            }

<<<<<<< HEAD

=======
>>>>>>> 5111ffc8aed24f24095f856baf4b0bb9cdf3c22e
            // get a search result that has primary key og keyword
            SearchResults searchResult = await _db.SearchResults.FindAsync(keyWord);
            var stockDList = new List<StockDetail>();

<<<<<<< HEAD

=======
>>>>>>> 5111ffc8aed24f24095f856baf4b0bb9cdf3c22e
            if (searchResult is null)
            {
                return null;
            }

            //go over the list of stock the searchreasult holds
            foreach (var searchRes in searchResult.Stocks)
            {

                //make each stock in to stockdetail object with ony neccessary properties extracted
                var stockDetail = new StockDetail()
                {
                    StockSymbol = searchRes.Symbol,
                    StockName = searchRes.StockName,
                };

                //add stockDetail in to a list
                stockDList.Add(stockDetail);

            };

            //from searchResults to searchResult object
            var dbSearchResult = new SearchResult()
            {
                SearchKeyword = searchResult.SearchKeyword,
                SearchTime = searchResult.SearchTimestamp,
                //list containing searchResult objects
                StockList = stockDList

            };

            return dbSearchResult;
<<<<<<< HEAD

=======
>>>>>>> 5111ffc8aed24f24095f856baf4b0bb9cdf3c22e
        }

       /**
       * This method saves SearchResult object by mapping to SearchResults table
       */
       public async Task<bool> SaveSearchResultAsync(SearchResult result)
        {
            
            var stocksList = new List<Stocks>();

            foreach (var stock in result.StockList)
            {
                var newStock = new Stocks();
                newStock.StockName = stock.StockName;
                newStock.Symbol = stock.StockSymbol;
                newStock.LastUpdated = stock.LastUpdated;
                newStock.Description = stock.Description;
                newStock.Currency = stock.Currency;
                stocksList.Add(newStock);
            }

            var dbSearchResult = new SearchResults();
            dbSearchResult.SearchKeyword = result.SearchKeyword;
            dbSearchResult.SearchTimestamp = DateTime.Now;
            dbSearchResult.Stocks = stocksList;

                _db.SearchResults.Add(dbSearchResult);
            await _db.SaveChangesAsync();


            return true;
        }

    /**
    * This method finds matching searchResult using keyword
    */
    public async Task<bool> FindMatchAsync(string? word)
     {
            try
            {
               var match = await _db.SearchResults.FindAsync(word);

                if(match != null)
                {
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                
                Debug.WriteLine(e.ToString());
                return false;

            }
        } 

    public void DeleteSearchResult(string keyword)
    {
        // This method removes all searchResults of the specified keyword. There should only be one quote stored for each stock
        var remove = _db.SearchResults.Where<SearchResults>(t => t.SearchKeyword == keyword);
        _db.SearchResults.RemoveRange(remove);
      }

    }
}
