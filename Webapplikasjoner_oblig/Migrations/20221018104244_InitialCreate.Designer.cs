﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Webapplikasjoner_oblig.DAL;

#nullable disable

namespace Webapplikasjoner_oblig.Migrations
{
    [DbContext(typeof(TradingContext))]
    [Migration("20221018104244_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.HasData(
                        new
                        {
                            SearchResultsSearchKeyword = "Microsoft",
                            StocksSymbol = "MSFT"
                        });
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

                    b.HasData(
                        new
                        {
                            FavoriteUsersUsersId = 1,
                            FavoritesSymbol = "MSFT"
                        });
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.SearchResults", b =>
                {
                    b.Property<string>("SearchKeyword")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SearchTimestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("SearchKeyword");

                    b.ToTable("SearchResults");

                    b.HasData(
                        new
                        {
                            SearchKeyword = "Microsoft",
                            SearchTimestamp = new DateTime(2022, 10, 18, 12, 42, 43, 969, DateTimeKind.Local).AddTicks(3051)
                        });
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

                    b.HasData(
                        new
                        {
                            UsersId = 1,
                            StocksId = "MSFT",
                            SpentValue = 100m,
                            StockCounter = 20
                        });
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.StockQuotes", b =>
                {
                    b.Property<string>("StocksId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<double>("Change")
                        .HasColumnType("REAL");

                    b.Property<string>("ChangePercent")
                        .HasColumnType("TEXT");

                    b.Property<double>("High")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("LatestTradingDay")
                        .HasColumnType("TEXT");

                    b.Property<double>("Low")
                        .HasColumnType("REAL");

                    b.Property<double>("Open")
                        .HasColumnType("REAL");

                    b.Property<double>("PreviousClose")
                        .HasColumnType("REAL");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("Volume")
                        .HasColumnType("INTEGER");

                    b.HasKey("StocksId", "Timestamp");

                    b.ToTable("StockQuotes");

                    b.HasData(
                        new
                        {
                            StocksId = "MSFT",
                            Timestamp = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Change = 0.35999999999999999,
                            ChangePercent = "0.2373%",
                            High = 153.41999999999999,
                            LatestTradingDay = new DateTime(2019, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Low = 151.02000000000001,
                            Open = 151.65000000000001,
                            PreviousClose = 151.69999999999999,
                            Price = 152.06,
                            Volume = 9425575
                        });
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.Stocks", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("TEXT");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("StockName")
                        .HasColumnType("TEXT");

                    b.HasKey("Symbol");

                    b.ToTable("Stocks");

                    b.HasData(
                        new
                        {
                            Symbol = "MSFT",
                            Currency = "USD",
                            Description = "Tech company",
                            LastUpdated = new DateTime(2022, 10, 18, 12, 42, 43, 969, DateTimeKind.Local).AddTicks(2996),
                            StockName = "Microsoft"
                        });
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

                    b.HasData(
                        new
                        {
                            TradesId = 1,
                            Currency = "NOK",
                            Saldo = 100m,
                            StockCount = 10,
                            StocksId = "MSFT",
                            TradeTime = new DateTime(2022, 10, 18, 12, 42, 43, 969, DateTimeKind.Local).AddTicks(4982),
                            UserIsBying = true,
                            UsersId = 1
                        });
                });

            modelBuilder.Entity("Webapplikasjoner_oblig.DAL.Users", b =>
                {
                    b.Property<int>("UsersId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("FundsAvailable")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("FundsSpent")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
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
                            FundsAvailable = 1000m,
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
                        .WithMany()
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
                        .HasForeignKey("StocksId");

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