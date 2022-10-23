namespace Webapplikasjoner_oblig.Model
{
    public class StockBase
    {
        public string Symbol { get; set; }
        public string StockName { get; set; }

        public string Description { get; set; }

        public string StockCurrency { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
