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

            options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase")).UseLazyLoadingProxies();


        }

        // https://learn.microsoft.com/en-us/ef/core/modeling/keys?tabs=data-annotations#configuring-a-primary-key
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stocks>().HasMany(c => c.StockQuotes).WithOne(c => c.Stocks);//.HasMany().WithOne(c => c.Stocks );
            modelBuilder.Entity<Stocks>().HasMany(c => c.StockOwnerships).WithOne(c => c.Stocks);
       
            modelBuilder.Entity<Users>().HasMany(c => c.StockOwnerships).WithOne(c => c.Users);//.HasMany(c => c.Trades).WithOne(c => c.Users );
            modelBuilder.Entity<Users>().HasOne(c => c.Favorites).WithOne(c => c.Users);
            modelBuilder.Entity<Trades>().HasOne(d => d.Stocks).WithMany(c => c.Trades);
            modelBuilder.Entity<Trades>().HasOne(d => d.Users).WithMany(c => c.Trades);

            modelBuilder.Entity<SearchResults>().HasMany(d => d.Stocks).WithMany(d => d.SearchResults);

            //modelBuilder.Entity<StockOccurances>().HasKey(c => new { c.SearchResultsId, c.StocksId });
            //modelBuilder.Entity<StockOccurances>().HasOne(d =>  d.Stocks ).HasOne(d => d.SearchResults).WithMany();

            modelBuilder.Entity<StockOwnerships>();
            modelBuilder.Entity<StockQuotes>();//.HasKey(c => new { c.Stocks, c.Timestamp });
            modelBuilder.Entity<Favorites>().HasOne(c => c.Users).WithOne(c => c.Favorites);
            modelBuilder.Entity<Favorites>().HasMany(c => c.Stocks).WithMany(c => c.Favorites);
        }

        // det er det som kobler til databasen

        public DbSet<Stocks> Stocks { get; set; }
        public DbSet<Users> Users { get; set; }

        public DbSet<Trades> Trades { get; set; }

        public DbSet<SearchResults> SearchResults { get; set; }

        public DbSet<Favorites> Favorites { get; set; }

        public DbSet<StockOwnerships> StockOwnerships { get; set; }

     }

    public class Stocks
    {
        [Key]
        public string Symbol { get; set; }

        public string StockName { get; set; }

        public string Description { get; set; }

        public DateTime LastUpdated { get; set; }

        virtual public List<Trades> Trades { get; set; }

        virtual public List<StockQuotes> StockQuotes { get; set; }

        virtual public List<StockOwnerships> StockOwnerships { get; set; }

        virtual public List<Favorites> Favorites { get; set; }

        virtual public List<SearchResults> SearchResults { get; set; }

    }

    /*public class StockOccurances
    {
        virtual public SearchResults SearchResultsId { get; set; }
        virtual public SearchResults SearchResults { get; set; }

        public int StocksId { get; set; }
        virtual public Stocks Stocks { get; set; }
    }*/

    public class SearchResults
    {
        [Key]
        public string SearchKeyword { get; set; }

        public DateTime SearchTimestamp {get; set;}

        virtual public List<Stocks> Stocks { get; set; }
    }

    public class Users
    {
        public int UsersId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public decimal FundsAvailable { get; set; }
        public decimal Fundsspent { get; set; }

        virtual public List<Trades> Trades { get; set; }

        virtual public List<StockOwnerships> StockOwnerships { get; set; }

        virtual public Favorites Favorites { get; set; }  
    }


    public class Trades
    {
        public int TradesId { get; set; }
        public int Count { get; set; }
        public DateTime Date { get; set; }
        public decimal Saldo { get; set; }
        public int StockId { get; set; }
        virtual public Stocks Stocks { get; set; }
        public int UsersId { get; set; }
        virtual public Users Users { get; set; }
    }

    public class StockOwnerships
    {
        public int StockOwnershipsId { get; set; }
        virtual public Users Users { get; set; }
        virtual public Stocks Stocks { get; set; }

        public int StockCount { get; set; }

    }

    public class Favorites
    {
        public int FavoritesId { get; set; }
        public int UsersId { get; set; }
        virtual public Users Users { get; set; }

        public int StocksId { get; set; }
        virtual public List<Stocks> Stocks { get; set; }

    }

    public class StockQuotes
    {
        public int StockQuotesId { get; set; }
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
        virtual public Stocks Stocks { get; set; }

    }
    


}
