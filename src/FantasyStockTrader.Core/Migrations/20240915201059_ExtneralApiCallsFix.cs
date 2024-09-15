using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyStockTrader.Core.Migrations
{
    /// <inheritdoc />
    public partial class ExtneralApiCallsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExternalApiCalls_AccountId",
                table: "ExternalApiCalls");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalApiCalls_AccountId",
                table: "ExternalApiCalls",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExternalApiCalls_AccountId",
                table: "ExternalApiCalls");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalApiCalls_AccountId",
                table: "ExternalApiCalls",
                column: "AccountId",
                unique: true);
        }
    }
}
