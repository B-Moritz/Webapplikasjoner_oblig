using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public interface ISearchResultRepositry
    {
        public Task<bool> SaveSearchResultAsync(SearchResult result);
        Task<List<SearchResult>> GetAllKeyWordsAsync();
        public Task<SearchResult>? GetOneKeyWordAsync(string keyWord);
        Task<bool> FindMatchAsync(string word);

        public void DeleteSearchResult(string symbol);
    }
}
