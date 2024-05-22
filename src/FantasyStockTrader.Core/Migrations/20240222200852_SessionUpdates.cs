using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyStockTrader.Core.Migrations
{
    /// <inheritdoc />
    public partial class SessionUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresAt",
                table: "Sessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Sessions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_AccountId",
                table: "Sessions",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Accounts_AccountId",
                table: "Sessions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Accounts_AccountId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_AccountId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Sessions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresAt",
                table: "Sessions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
