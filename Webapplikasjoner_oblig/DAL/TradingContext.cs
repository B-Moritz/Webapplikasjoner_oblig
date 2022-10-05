﻿using System.Reflection;
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
        public DbSet<StockDetail> stocks { get; set; }
        public DbSet<User> Users { get; set; }
        public object Trades { get; internal set; }
    }
}
