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
                SearchResults searchResult = await _db.SearchResults.FindAsync(keyWord);
                var stockDList = new List<StockDetail>();

                foreach (var searchRes in searchResult.Stocks)
                {

                    var stockDetail = new StockDetail()
                    {
                        Id = searchRes.Symbol,
                        StockName = searchRes.StockName,
                    };


                    stockDList.Add(stockDetail);

                };

                var dbSearchResult = new SearchResult()
                {
                    SearchKeyword = searchResult.SearchKeyword,
                    SearchTime = searchResult.SearchTimestamp,
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

        public Task<bool> SaveKeyWordAsync(string keyWord)
        {
            throw new NotImplementedException();
        }

        Task<string> ISearchResultRepositry.GetOneKeyWordAsync(string keyWord)
        {
            throw new NotImplementedException();
        }
    }
}
