using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapplikasjoner_oblig.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchResults",
                columns: table => new
                {
                    SearchKeyword = table.Column<string>(type: "TEXT", nullable: false),
                    SearchTimestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchResults", x => x.SearchKeyword);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    StockName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Symbol);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    FundsAvailable = table.Column<decimal>(type: "TEXT", nullable: false),
                    Fundsspent = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UsersId);
                });

            migrationBuilder.CreateTable(
                name: "SearchResultsStocks",
                columns: table => new
                {
                    SearchResultsSearchKeyword = table.Column<string>(type: "TEXT", nullable: false),
                    StocksSymbol = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchResultsStocks", x => new { x.SearchResultsSearchKeyword, x.StocksSymbol });
                    table.ForeignKey(
                        name: "FK_SearchResultsStocks_SearchResults_SearchResultsSearchKeyword",
                        column: x => x.SearchResultsSearchKeyword,
                        principalTable: "SearchResults",
                        principalColumn: "SearchKeyword",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SearchResultsStocks_Stocks_StocksSymbol",
                        column: x => x.StocksSymbol,
                        principalTable: "Stocks",
                        principalColumn: "Symbol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockQuotes",
                columns: table => new
                {
                    StockQuotesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", nullable: true),
                    Open = table.Column<double>(type: "REAL", nullable: false),
                    High = table.Column<double>(type: "REAL", nullable: false),
                    Low = table.Column<double>(type: "REAL", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Volume = table.Column<int>(type: "INTEGER", nullable: false),
                    LatestTradingDay = table.Column<string>(type: "TEXT", nullable: true),
                    PreviousClose = table.Column<double>(type: "REAL", nullable: false),
                    Change = table.Column<double>(type: "REAL", nullable: false),
                    ChangePercent = table.Column<string>(type: "TEXT", nullable: true),
                    StocksSymbol = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockQuotes", x => x.StockQuotesId);
                    table.ForeignKey(
                        name: "FK_StockQuotes_Stocks_StocksSymbol",
                        column: x => x.StocksSymbol,
                        principalTable: "Stocks",
                        principalColumn: "Symbol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    FavoritesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false),
                    StocksId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.FavoritesId);
                    table.ForeignKey(
                        name: "FK_Favorites_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "UsersId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockOwnerships",
                columns: table => new
                {
                    StockOwnershipsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false),
                    StocksSymbol = table.Column<string>(type: "TEXT", nullable: false),
                    StockCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOwnerships", x => x.StockOwnershipsId);
                    table.ForeignKey(
                        name: "FK_StockOwnerships_Stocks_StocksSymbol",
                        column: x => x.StocksSymbol,
                        principalTable: "Stocks",
                        principalColumn: "Symbol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockOwnerships_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "UsersId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    TradesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Saldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    StockId = table.Column<int>(type: "INTEGER", nullable: false),
                    StocksSymbol = table.Column<string>(type: "TEXT", nullable: false),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.TradesId);
                    table.ForeignKey(
                        name: "FK_Trades_Stocks_StocksSymbol",
                        column: x => x.StocksSymbol,
                        principalTable: "Stocks",
                        principalColumn: "Symbol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trades_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "UsersId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoritesStocks",
                columns: table => new
                {
                    FavoritesId = table.Column<int>(type: "INTEGER", nullable: false),
                    StocksSymbol = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritesStocks", x => new { x.FavoritesId, x.StocksSymbol });
                    table.ForeignKey(
                        name: "FK_FavoritesStocks_Favorites_FavoritesId",
                        column: x => x.FavoritesId,
                        principalTable: "Favorites",
                        principalColumn: "FavoritesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoritesStocks_Stocks_StocksSymbol",
                        column: x => x.StocksSymbol,
                        principalTable: "Stocks",
                        principalColumn: "Symbol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UsersId",
                table: "Favorites",
                column: "UsersId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoritesStocks_StocksSymbol",
                table: "FavoritesStocks",
                column: "StocksSymbol");

            migrationBuilder.CreateIndex(
                name: "IX_SearchResultsStocks_StocksSymbol",
                table: "SearchResultsStocks",
                column: "StocksSymbol");

            migrationBuilder.CreateIndex(
                name: "IX_StockOwnerships_StocksSymbol",
                table: "StockOwnerships",
                column: "StocksSymbol");

            migrationBuilder.CreateIndex(
                name: "IX_StockOwnerships_UsersId",
                table: "StockOwnerships",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_StockQuotes_StocksSymbol",
                table: "StockQuotes",
                column: "StocksSymbol");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_StocksSymbol",
                table: "Trades",
                column: "StocksSymbol");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_UsersId",
                table: "Trades",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoritesStocks");

            migrationBuilder.DropTable(
                name: "SearchResultsStocks");

            migrationBuilder.DropTable(
                name: "StockOwnerships");

            migrationBuilder.DropTable(
                name: "StockQuotes");

            migrationBuilder.DropTable(
                name: "Trades");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "SearchResults");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
