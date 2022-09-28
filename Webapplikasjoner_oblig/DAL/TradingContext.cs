using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public class TradingContext : DbContext
    {
       

        public TradingContext(DbContextOptions<TradingContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<StockDetails> stocks;
        public DbSet<User> users;
    }
}
