using Microsoft.EntityFrameworkCore;

namespace Webapplikasjoner_oblig.DAL
{
    public class TradingContext : DbContext
    {
        public class Stock
        {
            public string StockId { get; set; }
            public string StockName { get; set; }
            public string StockSymbol { get; set; }
            public string Description { get; set; }
        }

        

        public TradingContext(DbContextOptions<TradingContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
