namespace Webapplikasjoner_oblig.Model
{
    public class SearchResult
    {
        public string? SearchKeyword { get; set; }
        public DateTime SearchTime { get; set; }
        public List<StockSearchResult>? StockList { get; set; }

    }
}
