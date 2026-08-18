using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Updated_SMC_FUND_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbsmc_transactions_tbsmc_meeting_resolutions_resolutionid",
                table: "tbsmc_transactions");

            migrationBuilder.DropIndex(
                name: "IX_tbsmc_transactions_resolutionid",
                table: "tbsmc_transactions");

            migrationBuilder.RenameColumn(
                name: "resolutionid",
                table: "tbsmc_transactions",
                newName: "transactionid");

            migrationBuilder.AddColumn<Guid>(
                name: "meetingid",
                table: "tbsmc_transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_transactions_meetingid",
                table: "tbsmc_transactions",
                column: "meetingid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbsmc_transactions_tbsmc_meeting_meetingid",
                table: "tbsmc_transactions",
                column: "meetingid",
                principalTable: "tbsmc_meeting",
                principalColumn: "meetingid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbsmc_transactions_tbsmc_meeting_meetingid",
                table: "tbsmc_transactions");

            migrationBuilder.DropIndex(
                name: "IX_tbsmc_transactions_meetingid",
                table: "tbsmc_transactions");

            migrationBuilder.DropColumn(
                name: "meetingid",
                table: "tbsmc_transactions");

            migrationBuilder.RenameColumn(
                name: "transactionid",
                table: "tbsmc_transactions",
                newName: "resolutionid");

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_transactions_resolutionid",
                table: "tbsmc_transactions",
                column: "resolutionid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbsmc_transactions_tbsmc_meeting_resolutions_resolutionid",
                table: "tbsmc_transactions",
                column: "resolutionid",
                principalTable: "tbsmc_meeting_resolutions",
                principalColumn: "resolutionid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
