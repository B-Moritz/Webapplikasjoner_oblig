namespace Webapplikasjoner_oblig.Model
{
    public class Trade
    {
        public int Id { get; set; }
        public string StockSymbol { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
