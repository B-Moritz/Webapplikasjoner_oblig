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
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false)
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
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    FundsAvailable = table.Column<decimal>(type: "TEXT", nullable: false),
                    FundsSpent = table.Column<decimal>(type: "TEXT", nullable: false),
                    PortfolioCurrency = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UsersId);
                });

            migrationBuilder.CreateTable(
                name: "StockOccurances",
                columns: table => new
                {
                    SearchResultsSearchKeyword = table.Column<string>(type: "TEXT", nullable: false),
                    StocksSymbol = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOccurances", x => new { x.SearchResultsSearchKeyword, x.StocksSymbol });
                    table.ForeignKey(
                        name: "FK_StockOccurances_SearchResults_SearchResultsSearchKeyword",
                        column: x => x.SearchResultsSearchKeyword,
                        principalTable: "SearchResults",
                        principalColumn: "SearchKeyword",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockOccurances_Stocks_StocksSymbol",
                        column: x => x.StocksSymbol,
                        principalTable: "Stocks",
                        principalColumn: "Symbol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockQuotes",
                columns: table => new
                {
                    StocksId = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Open = table.Column<double>(type: "REAL", nullable: false),
                    High = table.Column<double>(type: "REAL", nullable: false),
                    Low = table.Column<double>(type: "REAL", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Volume = table.Column<int>(type: "INTEGER", nullable: false),
                    LatestTradingDay = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PreviousClose = table.Column<double>(type: "REAL", nullable: false),
                    Change = table.Column<double>(type: "REAL", nullable: false),
                    ChangePercent = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockQuotes", x => new { x.StocksId, x.Timestamp });
                    table.ForeignKey(
                        name: "FK_StockQuotes_Stocks_StocksId",
                        column: x => x.StocksId,
                        principalTable: "Stocks",
                        principalColumn: "Symbol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteLists",
                columns: table => new
                {
                    FavoriteUsersUsersId = table.Column<int>(type: "INTEGER", nullable: false),
                    FavoritesSymbol = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteLists", x => new { x.FavoriteUsersUsersId, x.FavoritesSymbol });
                    table.ForeignKey(
                        name: "FK_FavoriteLists_Stocks_FavoritesSymbol",
                        column: x => x.FavoritesSymbol,
                        principalTable: "Stocks",
                        principalColumn: "Symbol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteLists_Users_FavoriteUsersUsersId",
                        column: x => x.FavoriteUsersUsersId,
                        principalTable: "Users",
                        principalColumn: "UsersId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockOwnerships",
                columns: table => new
                {
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false),
                    StocksId = table.Column<string>(type: "TEXT", nullable: false),
                    StockCounter = table.Column<int>(type: "INTEGER", nullable: false),
                    SpentValue = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOwnerships", x => new { x.UsersId, x.StocksId });
                    table.ForeignKey(
                        name: "FK_StockOwnerships_Stocks_StocksId",
                        column: x => x.StocksId,
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
                    StockCount = table.Column<int>(type: "INTEGER", nullable: false),
                    TradeTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserIsBying = table.Column<bool>(type: "INTEGER", nullable: false),
                    Saldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    StocksId = table.Column<string>(type: "TEXT", nullable: true),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.TradesId);
                    table.ForeignKey(
                        name: "FK_Trades_Stocks_StocksId",
                        column: x => x.StocksId,
                        principalTable: "Stocks",
                        principalColumn: "Symbol");
                    table.ForeignKey(
                        name: "FK_Trades_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "UsersId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SearchResults",
                columns: new[] { "SearchKeyword", "SearchTimestamp" },
                values: new object[] { "Microsoft", new DateTime(2022, 10, 21, 23, 33, 42, 108, DateTimeKind.Local).AddTicks(8373) });

            migrationBuilder.InsertData(
                table: "Stocks",
                columns: new[] { "Symbol", "Currency", "Description", "LastUpdated", "StockName" },
                values: new object[] { "MSFT", "USD", "Tech company", new DateTime(2022, 10, 21, 23, 33, 42, 108, DateTimeKind.Local).AddTicks(8340), "Microsoft" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UsersId", "Email", "FirstName", "FundsAvailable", "FundsSpent", "LastName", "Password", "PortfolioCurrency" },
                values: new object[] { 1, "DevUser@test.com", "Dev", 1000000m, 100m, "User", "testpwd", "NOK" });

            migrationBuilder.InsertData(
                table: "FavoriteLists",
                columns: new[] { "FavoriteUsersUsersId", "FavoritesSymbol" },
                values: new object[] { 1, "MSFT" });

            migrationBuilder.InsertData(
                table: "StockOccurances",
                columns: new[] { "SearchResultsSearchKeyword", "StocksSymbol" },
                values: new object[] { "Microsoft", "MSFT" });

            migrationBuilder.InsertData(
                table: "StockOwnerships",
                columns: new[] { "StocksId", "UsersId", "SpentValue", "StockCounter" },
                values: new object[] { "MSFT", 1, 100m, 20 });

            migrationBuilder.InsertData(
                table: "StockQuotes",
                columns: new[] { "StocksId", "Timestamp", "Change", "ChangePercent", "High", "LatestTradingDay", "Low", "Open", "PreviousClose", "Price", "Volume" },
                values: new object[] { "MSFT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.35999999999999999, "0.2373%", 153.41999999999999, new DateTime(2019, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 151.02000000000001, 151.65000000000001, 151.69999999999999, 152.06, 9425575 });

            migrationBuilder.InsertData(
                table: "Trades",
                columns: new[] { "TradesId", "Currency", "Saldo", "StockCount", "StocksId", "TradeTime", "UserIsBying", "UsersId" },
                values: new object[] { 1, "NOK", 100m, 10, "MSFT", new DateTime(2022, 10, 21, 23, 33, 42, 108, DateTimeKind.Local).AddTicks(9170), true, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteLists_FavoritesSymbol",
                table: "FavoriteLists",
                column: "FavoritesSymbol");

            migrationBuilder.CreateIndex(
                name: "IX_StockOccurances_StocksSymbol",
                table: "StockOccurances",
                column: "StocksSymbol");

            migrationBuilder.CreateIndex(
                name: "IX_StockOwnerships_StocksId",
                table: "StockOwnerships",
                column: "StocksId");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_StocksId",
                table: "Trades",
                column: "StocksId");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_UsersId",
                table: "Trades",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteLists");

            migrationBuilder.DropTable(
                name: "StockOccurances");

            migrationBuilder.DropTable(
                name: "StockOwnerships");

            migrationBuilder.DropTable(
                name: "StockQuotes");

            migrationBuilder.DropTable(
                name: "Trades");

            migrationBuilder.DropTable(
                name: "SearchResults");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
