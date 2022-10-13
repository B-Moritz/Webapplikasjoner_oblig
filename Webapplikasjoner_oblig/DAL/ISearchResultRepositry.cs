namespace Webapplikasjoner_oblig.DAL
{
    public interface ISearchResultRepositry
    {
        Task<bool> SaveKeyWordAsync(string keyWord);
        Task<List<string>> GetAllKeyWordsAsync();
        Task<string> GetOneKeyWordAsync(string keyWord);
    }
}
