using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapplikasjoner_oblig.Migrations
{
    public partial class AddCurrencyPropertyInTrades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Trades",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "SearchResults",
                keyColumn: "SearchKeyword",
                keyValue: "Microsoft",
                column: "SearchTimestamp",
                value: new DateTime(2022, 10, 14, 13, 2, 9, 3, DateTimeKind.Local).AddTicks(5042));

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
                columns: new[] { "Currency", "TradeTime", "UserIsBying" },
                values: new object[] { "NOK", new DateTime(2022, 10, 14, 13, 2, 9, 3, DateTimeKind.Local).AddTicks(7777), true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Trades");

            migrationBuilder.UpdateData(
                table: "SearchResults",
                keyColumn: "SearchKeyword",
                keyValue: "Microsoft",
                column: "SearchTimestamp",
                value: new DateTime(2022, 10, 14, 12, 1, 12, 637, DateTimeKind.Local).AddTicks(3720));

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "Symbol",
                keyValue: "MSFT",
                column: "LastUpdated",
                value: new DateTime(2022, 10, 14, 12, 1, 12, 637, DateTimeKind.Local).AddTicks(3667));

            migrationBuilder.UpdateData(
                table: "Trades",
                keyColumn: "TradesId",
                keyValue: 1,
                columns: new[] { "TradeTime", "UserIsBying" },
                values: new object[] { new DateTime(2022, 10, 14, 12, 1, 12, 637, DateTimeKind.Local).AddTicks(5171), false });
        }
    }
}
