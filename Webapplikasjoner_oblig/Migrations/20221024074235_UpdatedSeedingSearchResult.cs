using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapplikasjoner_oblig.Migrations
{
    public partial class UpdatedSeedingSearchResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SearchResults",
                keyColumn: "SearchKeyword",
                keyValue: "Microsoft");

            migrationBuilder.InsertData(
                table: "SearchResults",
                columns: new[] { "SearchKeyword", "SearchTimestamp" },
                values: new object[] { "MICROSOFT", new DateTime(2022, 10, 24, 9, 42, 35, 445, DateTimeKind.Local).AddTicks(3024) });

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "Symbol",
                keyValue: "MSFT",
                columns: new[] { "LastUpdated", "StockName" },
                values: new object[] { new DateTime(2022, 10, 24, 9, 42, 35, 445, DateTimeKind.Local).AddTicks(2989), "Microsoft" });

            migrationBuilder.UpdateData(
                table: "Trades",
                keyColumn: "TradesId",
                keyValue: 1,
                column: "TradeTime",
                value: new DateTime(2022, 10, 24, 9, 42, 35, 445, DateTimeKind.Local).AddTicks(3761));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SearchResults",
                keyColumn: "SearchKeyword",
                keyValue: "MICROSOFT");

            migrationBuilder.InsertData(
                table: "SearchResults",
                columns: new[] { "SearchKeyword", "SearchTimestamp" },
                values: new object[] { "Microsoft", new DateTime(2022, 10, 24, 9, 38, 13, 739, DateTimeKind.Local).AddTicks(5261) });

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "Symbol",
                keyValue: "MSFT",
                columns: new[] { "LastUpdated", "StockName" },
                values: new object[] { new DateTime(2022, 10, 24, 9, 38, 13, 739, DateTimeKind.Local).AddTicks(5228), "MICROSOFT" });

            migrationBuilder.UpdateData(
                table: "Trades",
                keyColumn: "TradesId",
                keyValue: 1,
                column: "TradeTime",
                value: new DateTime(2022, 10, 24, 9, 38, 13, 739, DateTimeKind.Local).AddTicks(5997));
        }
    }
}
