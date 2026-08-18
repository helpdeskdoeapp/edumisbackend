using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updated_foreignkeys_in_smc_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbsmc_transactions",
                table: "tbsmc_transactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbsmc_transactions",
                table: "tbsmc_transactions",
                column: "transactionid");

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_transactions_meetingid",
                table: "tbsmc_transactions",
                column: "meetingid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbsmc_trans_attachments_tbsmc_transactions_transactionid",
                table: "tbsmc_trans_attachments",
                column: "transactionid",
                principalTable: "tbsmc_transactions",
                principalColumn: "transactionid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbsmc_trans_attachments_tbsmc_transactions_transactionid",
                table: "tbsmc_trans_attachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbsmc_transactions",
                table: "tbsmc_transactions");

            migrationBuilder.DropIndex(
                name: "IX_tbsmc_transactions_meetingid",
                table: "tbsmc_transactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbsmc_transactions",
                table: "tbsmc_transactions",
                columns: new[] { "meetingid", "transactionid" });
        }
    }
}
