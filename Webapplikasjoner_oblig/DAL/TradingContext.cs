using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webapplikasjoner_oblig.DAL
{
   

    public class TradingContext : DbContext
    {
        protected readonly IConfiguration Configuration;


        public TradingContext(IConfiguration configuration)
        {
           Configuration = configuration;
        }


        //Migrations microsoft tutorial 
        //https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

        //.net core web api microsoft tutorial
        //https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
        }

        // det er det som kobler til databasen

        public DbSet<Stocks> Stocks { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Trade> Trades { get; set; }
     }

    public class Stocks
    {
        [Key]
        public string Symbol { get; set; }

        public string StockName { get; set; }

        public string Description { get; set; }

        public DateTime lastUpdated { get; set; }
    }

    public class StockOccurances
    {
        [Key]
        public string SearchKeyword { get; set; }
        public Stocks StockSymbol { get; set; }
    }

    public class SearchResults
    {
        [Key]
        public string SearchKeyword { get; set; }

        public DateTime SearchTimestamp {get; set;}

    }

}
