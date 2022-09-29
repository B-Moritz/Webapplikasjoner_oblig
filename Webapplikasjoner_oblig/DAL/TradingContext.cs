using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public class TradingContext : DbContext
    {
        protected readonly IConfiguration Configuration;


        public TradingContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<StockDetails> Stocks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
