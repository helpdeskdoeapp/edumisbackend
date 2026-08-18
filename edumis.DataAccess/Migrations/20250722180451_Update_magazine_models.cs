using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Update_magazine_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbsmc_trans_attachments_tbsmc_transactions_SMCFundTransacti~",
                table: "tbsmc_trans_attachments");

            migrationBuilder.DropIndex(
                name: "IX_tbsmc_trans_attachments_SMCFundTransactionsModelTransaction~",
                table: "tbsmc_trans_attachments");

            migrationBuilder.DropColumn(
                name: "SMCFundTransactionsModelTransactionId",
                table: "tbsmc_trans_attachments");

            migrationBuilder.AddColumn<int>(
                name: "availableqty",
                table: "tbbk_magazine",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "availableqty",
                table: "tbbk_magazine");

            migrationBuilder.AddColumn<Guid>(
                name: "SMCFundTransactionsModelTransactionId",
                table: "tbsmc_trans_attachments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_trans_attachments_SMCFundTransactionsModelTransaction~",
                table: "tbsmc_trans_attachments",
                column: "SMCFundTransactionsModelTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbsmc_trans_attachments_tbsmc_transactions_SMCFundTransacti~",
                table: "tbsmc_trans_attachments",
                column: "SMCFundTransactionsModelTransactionId",
                principalTable: "tbsmc_transactions",
                principalColumn: "transactionid");
        }
    }
}
