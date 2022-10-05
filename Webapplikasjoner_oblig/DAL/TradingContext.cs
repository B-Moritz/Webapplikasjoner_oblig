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
            options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"))
                   .UseLazyLoadingProxies();
        }

        // https://learn.microsoft.com/en-us/ef/core/modeling/keys?tabs=data-annotations#configuring-a-primary-key
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockOccurances>().HasKey(c => new { c.SearchKeyword, c.StockSymbol });
            modelBuilder.Entity<StockOwnerships>().HasKey(c => new { c.UserId, c.Symbol });
            modelBuilder.Entity<StockQuotes>().HasKey(c => new { c.StockSymbol, c.Timestamp });
            modelBuilder.Entity<Favorites>().HasKey(c => new { c.UserId, c.Symbol });
        }

        // det er det som kobler til databasen

        public DbSet<Stocks> Stocks { get; set; }
        public DbSet<Users> Users { get; set; }

        public DbSet<Trade> Trades { get; set; }

        public DbSet<SearchResults> SearchResults { get; set; }
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
        virtual public SearchResult SearchKeyword { get; set; }
        virtual public Stocks StockSymbol { get; set; }
    }

    public class SearchResults
    {
        [Key]
        public string SearchKeyword { get; set; }

        public DateTime SearchTimestamp {get; set;}
    }

    public class Users
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public decimal FundsAvailable { get; set; }
        public decimal Fundsspent { get; set; }
    }


    public class Trades
    {

        public int Id { get; set; }
        public string StockSymbol { get; set; }
        public DateTime Date { get; set; }

        virtual public Stocks Symbol { get; set; }

        virtual public Users UserId { get; set; }


    }
    //kommentar

    public class StockOwnerships
    {
        virtual public Users UserId { get; set; }
        virtual public Stocks Symbol { get; set; }

        public int StockCount { get; set; }

    }

    public class Favorites
    {
        virtual public Users UserId { get; set; }
        virtual public Stocks Symbol { get; set; }

    }

    public class StockQuotes
    {
        virtual public Stocks StockSymbol { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Symbol { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Price { get; set; }
        public int Volume { get; set; }
        public string? LatestTradingDay { get; set; }
        public double PreviousClose { get; set; }
        public double Change { get; set; }
        public string? ChangePercent { get; set; }

    }
    


}
