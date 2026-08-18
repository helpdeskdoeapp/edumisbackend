using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Resolution_SMC_Model_Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbsmc_transactions_tbsmc_meeting_meetingid",
                table: "tbsmc_transactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "meetingid",
                table: "tbsmc_transactions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "SMCFundTransactionsModelTransactionId",
                table: "tbsmc_trans_attachments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "actualcost",
                table: "tbsmc_meeting_resolutions",
                type: "numeric",
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

            migrationBuilder.AddForeignKey(
                name: "FK_tbsmc_transactions_tbsmc_meeting_meetingid",
                table: "tbsmc_transactions",
                column: "meetingid",
                principalTable: "tbsmc_meeting",
                principalColumn: "meetingid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbsmc_trans_attachments_tbsmc_transactions_SMCFundTransacti~",
                table: "tbsmc_trans_attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_tbsmc_transactions_tbsmc_meeting_meetingid",
                table: "tbsmc_transactions");

            migrationBuilder.DropIndex(
                name: "IX_tbsmc_trans_attachments_SMCFundTransactionsModelTransaction~",
                table: "tbsmc_trans_attachments");

            migrationBuilder.DropColumn(
                name: "SMCFundTransactionsModelTransactionId",
                table: "tbsmc_trans_attachments");

            migrationBuilder.DropColumn(
                name: "actualcost",
                table: "tbsmc_meeting_resolutions");

            migrationBuilder.AlterColumn<Guid>(
                name: "meetingid",
                table: "tbsmc_transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbsmc_transactions_tbsmc_meeting_meetingid",
                table: "tbsmc_transactions",
                column: "meetingid",
                principalTable: "tbsmc_meeting",
                principalColumn: "meetingid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
