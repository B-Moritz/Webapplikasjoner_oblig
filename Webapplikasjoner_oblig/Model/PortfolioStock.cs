namespace Webapplikasjoner_oblig.Model
{
    public class PortfolioStock : StockBase
    {
        public int StockCounter { get; set; }
        public decimal TotalValue { get; set; }

        public decimal TotalFundsSpent { get; set; }
    }
}
