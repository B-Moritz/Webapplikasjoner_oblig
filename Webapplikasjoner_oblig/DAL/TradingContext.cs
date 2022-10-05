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

        // https://learn.microsoft.com/en-us/ef/core/modeling/keys?tabs=data-annotations#configuring-a-primary-key
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockOccurances>().HasKey(c => new { c.SearchKeyword, c.StockSymbol });
<<<<<<< HEAD
            modelBuilder.Entity<StockOwnerships>().HasKey(c => new { c.UserId, c.Symbol });
=======

>>>>>>> 496b02fe716fa6854eed1e52a905e33abdffe0c5
        }

        // det er det som kobler til databasen

        public DbSet<Stocks> Stocks { get; set; }
        public DbSet<User> Users { get; set; }

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

    


}
