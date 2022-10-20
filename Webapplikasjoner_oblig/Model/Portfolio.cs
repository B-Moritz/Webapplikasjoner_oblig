namespace Webapplikasjoner_oblig.Model
{
    public class Portfolio
    {
        public DateTime LastUpdate { get; set; }
        public string TotalValueSpent { get; set; }
        // The estimated market value of the portfolio
        public string EstPortfolioValue { get; set; }
        // The currency used to show the values
        public string PortfolioCurrency { get; set; }
        // The portfolio stocks
        public List<PortfolioStock> Stocks { get; set; }
        // The total funds available to use by the user
        public string BuyingPower { get; set; }

        // The unrealized profit & loss for the entire portfolio
        public string UnrealizedPL { get; set; }
    }
}
