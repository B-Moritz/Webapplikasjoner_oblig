using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapplikasjoner_oblig.Migrations
{
    public partial class AddingOwnership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SearchResults",
                keyColumn: "SearchKeyword",
                keyValue: "Microsoft",
                column: "SearchTimestamp",
                value: new DateTime(2022, 10, 14, 13, 21, 32, 187, DateTimeKind.Local).AddTicks(3481));

            migrationBuilder.UpdateData(
                table: "StockOwnerships",
                keyColumns: new[] { "StocksId", "UsersId" },
                keyValues: new object[] { "MSFT", 1 },
                column: "StockCounter",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "Symbol",
                keyValue: "MSFT",
                column: "LastUpdated",
                value: new DateTime(2022, 10, 14, 13, 21, 32, 187, DateTimeKind.Local).AddTicks(3428));

            migrationBuilder.UpdateData(
                table: "Trades",
                keyColumn: "TradesId",
                keyValue: 1,
                column: "TradeTime",
                value: new DateTime(2022, 10, 14, 13, 21, 32, 187, DateTimeKind.Local).AddTicks(5008));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SearchResults",
                keyColumn: "SearchKeyword",
                keyValue: "Microsoft",
                column: "SearchTimestamp",
                value: new DateTime(2022, 10, 14, 13, 2, 9, 3, DateTimeKind.Local).AddTicks(5042));

            migrationBuilder.UpdateData(
                table: "StockOwnerships",
                keyColumns: new[] { "StocksId", "UsersId" },
                keyValues: new object[] { "MSFT", 1 },
                column: "StockCounter",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "Symbol",
                keyValue: "MSFT",
                column: "LastUpdated",
                value: new DateTime(2022, 10, 14, 13, 2, 9, 3, DateTimeKind.Local).AddTicks(4977));

            migrationBuilder.UpdateData(
                table: "Trades",
                keyColumn: "TradesId",
                keyValue: 1,
                column: "TradeTime",
                value: new DateTime(2022, 10, 14, 13, 2, 9, 3, DateTimeKind.Local).AddTicks(7777));
        }
    }
}
