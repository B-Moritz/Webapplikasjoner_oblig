namespace Webapplikasjoner_oblig.Model
{
    public class Trade
    {
        public int Id { get; set; }
        public string StockSymbol { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        // legger noen atributer
        public bool UserBuying { get; set; }
        public int StockCount { get; set; }
        public decimal Price { get; set; }

    }
}