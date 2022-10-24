using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapplikasjoner_oblig.Migrations
{
    public partial class UpdatedSeedings3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UsersId", "Email", "FirstName", "FundsAvailable", "FundsSpent", "LastName", "Password", "PortfolioCurrency" },
                values: new object[] { 1, "DevUser@test.com", "Dev", 1000000m, 100m, "User", "testpwd", "NOK" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UsersId",
                keyValue: 1);
        }
    }
}
