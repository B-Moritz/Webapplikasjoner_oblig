﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Webapplikasjoner_oblig.DAL;

#nullable disable

namespace Webapplikasjoner_oblig.Migrations
{
    [DbContext(typeof(TradingContext))]
    partial class TradingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.9");

            modelBuilder.Entity("SearchResultsStocks", b =>
                {
                    b.Property<string>("SearchResultsSearchKeyword")
                        .HasColumnType("TEXT");

                    b.Property<string>("StocksSymbol")
                        .HasColumnType("TEXT");

                    b.HasKey("SearchResultsSearchKeyword", "StocksSymbol");

                    b.HasIndex("StocksSymbol");

                    b.ToTable("StockOccurances", (string)null);
                });

            modelBuilder.Entity("StocksUsers", b =>
                {
                    b.Property<int>("FavoriteUsersUsersId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FavoritesSymbol")
                        .HasColumnType("TEXT");

                    b.HasKey("FavoriteUsersUsersId", "FavoritesSymbol");

                    b.HasIndex("FavoritesSymbol");

                    b.ToTable("FavoriteLists", (string)null);
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.SearchResults", b =>
                {
                    b.Property<string>("SearchKeyword")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SearchTimestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("SearchKeyword");

                    b.ToTable("SearchResults");
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.StockOwnerships", b =>
                {
                    b.Property<int>("UsersId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StocksId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SpentValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("StockCounter")
                        .HasColumnType("INTEGER");

                    b.HasKey("UsersId", "StocksId");

                    b.HasIndex("StocksId");

                    b.ToTable("StockOwnerships");
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.StockQuotes", b =>
                {
                    b.Property<string>("StocksId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Change")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChangePercent")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("High")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LatestTradingDay")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Low")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Open")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PreviousClose")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("Volume")
                        .HasColumnType("INTEGER");

                    b.HasKey("StocksId", "Timestamp");

                    b.ToTable("StockQuotes");
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.Stocks", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("TEXT");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("StockName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Symbol");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.Trades", b =>
                {
                    b.Property<int>("TradesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Saldo")
                        .HasColumnType("TEXT");

                    b.Property<int>("StockCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StocksId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TradeTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("UserIsBying")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsersId")
                        .HasColumnType("INTEGER");

                    b.HasKey("TradesId");

                    b.HasIndex("StocksId");

                    b.HasIndex("UsersId");

                    b.ToTable("Trades");
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.Users", b =>
                {
                    b.Property<int>("UsersId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("FundsAvailable")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("FundsSpent")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PortfolioCurrency")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UsersId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UsersId = 1,
                            Email = "DevUser@test.com",
                            FirstName = "Dev",
                            FundsAvailable = 1000000m,
                            FundsSpent = 0m,
                            LastName = "User",
                            Password = "testpwd",
                            PortfolioCurrency = "NOK"
                        });
                });

            modelBuilder.Entity("SearchResultsStocks", b =>
                {
                    b.HasOne("Webapplikasjoner_oblig.DAL.SearchResults", null)
                        .WithMany()
                        .HasForeignKey("SearchResultsSearchKeyword")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webapplikasjoner_oblig.DAL.Stocks", null)
                        .WithMany()
                        .HasForeignKey("StocksSymbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StocksUsers", b =>
                {
                    b.HasOne("Webapplikasjoner_oblig.DAL.Users", null)
                        .WithMany()
                        .HasForeignKey("FavoriteUsersUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webapplikasjoner_oblig.DAL.Stocks", null)
                        .WithMany()
                        .HasForeignKey("FavoritesSymbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.StockOwnerships", b =>
                {
                    b.HasOne("Webapplikasjoner_oblig.DAL.Stocks", "Stock")
                        .WithMany("Owners")
                        .HasForeignKey("StocksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webapplikasjoner_oblig.DAL.Users", "User")
                        .WithMany("Portfolio")
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.StockQuotes", b =>
                {
                    b.HasOne("Webapplikasjoner_oblig.DAL.Stocks", "Stock")
                        .WithMany("StockQuotes")
                        .HasForeignKey("StocksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.Trades", b =>
                {
                    b.HasOne("Webapplikasjoner_oblig.DAL.Stocks", "Stock")
                        .WithMany("TradeOccurances")
                        .HasForeignKey("StocksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Webapplikasjoner_oblig.DAL.Users", "User")
                        .WithMany("Trades")
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.Stocks", b =>
                {
                    b.Navigation("Owners");

                    b.Navigation("StockQuotes");

                    b.Navigation("TradeOccurances");
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.Users", b =>
                {
                    b.Navigation("Portfolio");

                    b.Navigation("Trades");
                });
#pragma warning restore 612, 618
        }
    }
}
