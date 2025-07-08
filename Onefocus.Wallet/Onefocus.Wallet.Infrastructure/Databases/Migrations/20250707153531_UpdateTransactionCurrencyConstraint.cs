using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onefocus.Wallet.Infrastructure.Databases.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionCurrencyConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Currency_OwnerUserId",
                table: "Transaction");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CurrencyId",
                table: "Transaction",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Currency_CurrencyId",
                table: "Transaction",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Currency_CurrencyId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_CurrencyId",
                table: "Transaction");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Currency_OwnerUserId",
                table: "Transaction",
                column: "OwnerUserId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
