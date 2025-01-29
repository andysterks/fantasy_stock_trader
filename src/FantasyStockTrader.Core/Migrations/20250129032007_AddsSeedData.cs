using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyStockTrader.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddsSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "EmailAddress", "FirstName", "LastName", "Password" },
                values: new object[] { new Guid("eb0e7bd5-df42-46cc-bbe7-7ecb8d8718d9"), new DateTime(2025, 1, 29, 0, 0, 34, 0, DateTimeKind.Unspecified), null, "andy@email.com", "Andy", "Sterkowitz", "$2b$10$JKnwr5mA2ux4iN1RXbAVC.92tIwUrjmxiOZfG1DDK/GOtwkPl/7p6" });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "AccountId", "Amount", "CreatedAt", "Price", "Symbol", "Type" },
                values: new object[] { new Guid("781073a5-3f84-46c6-a3c1-5fb21568525e"), new Guid("eb0e7bd5-df42-46cc-bbe7-7ecb8d8718d9"), 100, new DateTime(2025, 1, 29, 0, 2, 54, 0, DateTimeKind.Unspecified), 398.08999999999997, "TSLA", "BUY" });

            migrationBuilder.InsertData(
                table: "Holdings",
                columns: new[] { "Id", "AccountId", "CostBasis", "CreatedAt", "Shares", "Symbol" },
                values: new object[] { new Guid("c38133e9-d75c-42d6-a537-36d458c96cd9"), new Guid("eb0e7bd5-df42-46cc-bbe7-7ecb8d8718d9"), 39809.0, new DateTime(2025, 1, 29, 0, 2, 54, 0, DateTimeKind.Unspecified), 100, "TSLA" });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "AccountId", "Amount", "UpdatedAt" },
                values: new object[] { new Guid("579bb084-88cf-438b-85a1-b0822b731f32"), new Guid("eb0e7bd5-df42-46cc-bbe7-7ecb8d8718d9"), 60191m, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Holdings",
                keyColumn: "Id",
                keyValue: new Guid("c38133e9-d75c-42d6-a537-36d458c96cd9"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: new Guid("781073a5-3f84-46c6-a3c1-5fb21568525e"));

            migrationBuilder.DeleteData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: new Guid("579bb084-88cf-438b-85a1-b0822b731f32"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("eb0e7bd5-df42-46cc-bbe7-7ecb8d8718d9"));
        }
    }
}
