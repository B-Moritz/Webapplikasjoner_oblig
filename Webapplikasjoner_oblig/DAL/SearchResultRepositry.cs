using Webapplikasjoner_oblig.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PeanutButter.Utils;

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
            try
            {
                //list of all search results 
                var keywords = await _db.SearchResults.ToListAsync();

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
                   foreach(var stock in keyword.Stocks)
                    {
                        //mapping stock to stockDetails
                        var stockDetail = new StockDetail()
                        {
                            StockSymbol = stock.Symbol,
                            StockName = stock.StockName,
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }




        public async Task<SearchResult>? GetOneKeyWordAsync(string keyWord)
        {
            if (keyWord == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                // get a search result that has primary key og keyword
                SearchResults searchResult = await _db.SearchResults.FindAsync(keyWord);
                var stockDList = new List<StockDetail>();

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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }



        Task<bool> ISearchResultRepositry.SaveKeyWordAsync(string keyWord)
        {
            throw new NotImplementedException();
        }
    }

}
