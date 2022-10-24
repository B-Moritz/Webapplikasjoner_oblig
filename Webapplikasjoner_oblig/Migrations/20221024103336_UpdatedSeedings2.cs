using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapplikasjoner_oblig.Migrations
{
    public partial class UpdatedSeedings2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FavoriteLists",
                keyColumns: new[] { "FavoriteUsersUsersId", "FavoritesSymbol" },
                keyValues: new object[] { 1, "MSFT" });

            migrationBuilder.DeleteData(
                table: "SearchResults",
                keyColumn: "SearchKeyword",
                keyValue: "MICROSOFT");

            migrationBuilder.DeleteData(
                table: "StockOccurances",
                keyColumns: new[] { "SearchResultsSearchKeyword", "StocksSymbol" },
                keyValues: new object[] { "Microsoft", "MSFT" });

            migrationBuilder.DeleteData(
                table: "StockOwnerships",
                keyColumns: new[] { "StocksId", "UsersId" },
                keyValues: new object[] { "MSFT", 1 });

            migrationBuilder.DeleteData(
                table: "StockQuotes",
                keyColumns: new[] { "StocksId", "Timestamp" },
                keyValues: new object[] { "MSFT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.DeleteData(
                table: "Trades",
                keyColumn: "TradesId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Stocks",
                keyColumn: "Symbol",
                keyValue: "MSFT");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UsersId",
                keyValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SearchResults",
                columns: new[] { "SearchKeyword", "SearchTimestamp" },
                values: new object[] { "MICROSOFT", new DateTime(2022, 10, 24, 12, 28, 44, 360, DateTimeKind.Local).AddTicks(1508) });

            migrationBuilder.InsertData(
                table: "Stocks",
                columns: new[] { "Symbol", "Currency", "Description", "LastUpdated", "StockName" },
                values: new object[] { "MSFT", "USD", "Tech company", new DateTime(2022, 10, 24, 12, 28, 44, 360, DateTimeKind.Local).AddTicks(1476), "Microsoft" });

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
                values: new object[] { 1, "NOK", 100m, 10, "MSFT", new DateTime(2022, 10, 24, 12, 28, 44, 360, DateTimeKind.Local).AddTicks(2243), true, 1 });
        }
    }
}
