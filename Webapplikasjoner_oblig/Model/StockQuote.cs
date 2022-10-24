using Webapplikasjoner_oblig.DAL;

namespace Webapplikasjoner_oblig.Model
{
    public class StockQuote : StockBase
    {
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Price { get; set; }
        public int Volume { get; set; }
        public DateTime? LatestTradingDay { get; set; }
        public string PreviousClose { get; set; }
        public string Change { get; set; }
        public string ChangePercent { get; set; }
    }
}
