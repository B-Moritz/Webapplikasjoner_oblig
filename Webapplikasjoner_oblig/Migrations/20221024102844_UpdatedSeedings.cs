using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapplikasjoner_oblig.Migrations
{
    public partial class UpdatedSeedings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SearchResults",
                keyColumn: "SearchKeyword",
                keyValue: "MICROSOFT",
                column: "SearchTimestamp",
                value: new DateTime(2022, 10, 24, 12, 28, 44, 360, DateTimeKind.Local).AddTicks(1508));

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "Symbol",
                keyValue: "MSFT",
                column: "LastUpdated",
                value: new DateTime(2022, 10, 24, 12, 28, 44, 360, DateTimeKind.Local).AddTicks(1476));

            migrationBuilder.UpdateData(
                table: "Trades",
                keyColumn: "TradesId",
                keyValue: 1,
                column: "TradeTime",
                value: new DateTime(2022, 10, 24, 12, 28, 44, 360, DateTimeKind.Local).AddTicks(2243));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SearchResults",
                keyColumn: "SearchKeyword",
                keyValue: "MICROSOFT",
                column: "SearchTimestamp",
                value: new DateTime(2022, 10, 24, 9, 42, 35, 445, DateTimeKind.Local).AddTicks(3024));

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "Symbol",
                keyValue: "MSFT",
                column: "LastUpdated",
                value: new DateTime(2022, 10, 24, 9, 42, 35, 445, DateTimeKind.Local).AddTicks(2989));

            migrationBuilder.UpdateData(
                table: "Trades",
                keyColumn: "TradesId",
                keyValue: 1,
                column: "TradeTime",
                value: new DateTime(2022, 10, 24, 9, 42, 35, 445, DateTimeKind.Local).AddTicks(3761));
        }
    }
}
