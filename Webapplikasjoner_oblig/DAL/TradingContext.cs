using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        virtual public Users Ussers { get; set; }

    }

    public class TradingContext : DbContext
    {
       

        public TradingContext(DbContextOptions<TradingContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<StockDetails> stocks;
        //public DbSet<User> users;
        public DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
}
