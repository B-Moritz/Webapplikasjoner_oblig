using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public class SearchResultRepositry : ISearchResultRepositry
    {
        private readonly TradingContext _db;

        
        public Task<List<string>> GetAllKeyWordsAsync()
        {
            throw new NotImplementedException();
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
