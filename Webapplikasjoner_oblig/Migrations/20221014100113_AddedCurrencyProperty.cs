using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapplikasjoner_oblig.Migrations
{
    public partial class AddedCurrencyProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PortfolioCurrency",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Stocks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

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
                columns: new[] { "Currency", "LastUpdated" },
                values: new object[] { "USD", new DateTime(2022, 10, 14, 12, 1, 12, 637, DateTimeKind.Local).AddTicks(3667) });

            migrationBuilder.UpdateData(
                table: "Trades",
                keyColumn: "TradesId",
                keyValue: 1,
                column: "TradeTime",
                value: new DateTime(2022, 10, 14, 12, 1, 12, 637, DateTimeKind.Local).AddTicks(5171));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UsersId",
                keyValue: 1,
                column: "PortfolioCurrency",
                value: "NOK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PortfolioCurrency",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Stocks");

            migrationBuilder.UpdateData(
                table: "SearchResults",
                keyColumn: "SearchKeyword",
                keyValue: "Microsoft",
                column: "SearchTimestamp",
                value: new DateTime(2022, 10, 6, 23, 13, 47, 987, DateTimeKind.Local).AddTicks(978));

            migrationBuilder.UpdateData(
                table: "Stocks",
                keyColumn: "Symbol",
                keyValue: "MSFT",
                column: "LastUpdated",
                value: new DateTime(2022, 10, 6, 23, 13, 47, 987, DateTimeKind.Local).AddTicks(941));

            migrationBuilder.UpdateData(
                table: "Trades",
                keyColumn: "TradesId",
                keyValue: 1,
                column: "TradeTime",
                value: new DateTime(2022, 10, 6, 23, 13, 47, 987, DateTimeKind.Local).AddTicks(1744));
        }
    }
}
