namespace Webapplikasjoner_oblig.Model
{
    public class Portfolio
    {
        public DateTime LastUpdate { get; set; }
        public decimal TotalValueSpent { get; set; }
        public decimal TotalPortfolioValue { get; set; }
        public string PortfolioCurrency { get; set; }
        public List<PortfolioStock> Stocks { get; set; }
    }
}
