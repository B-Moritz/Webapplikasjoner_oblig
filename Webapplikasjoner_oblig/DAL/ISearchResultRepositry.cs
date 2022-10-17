using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public interface ISearchResultRepositry
    {
        public Task<bool> SaveKeyWordAsync(string keyWord);
        Task<List<SearchResult>> GetAllKeyWordsAsync();
        public Task<SearchResult>? GetOneKeyWordAsync(string keyWord);
    }
}
