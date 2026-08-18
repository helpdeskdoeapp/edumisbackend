using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Update_SMC_Trans_Models2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbsmc_transactions",
                table: "tbsmc_transactions");

            migrationBuilder.DropIndex(
                name: "IX_tbsmc_transactions_meetingid",
                table: "tbsmc_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbsmc_trans_attachments",
                table: "tbsmc_trans_attachments");

            migrationBuilder.DropColumn(
                name: "transid",
                table: "tbsmc_trans_attachments");

            migrationBuilder.AlterColumn<string>(
                name: "filepath",
                table: "tbsmc_trans_attachments",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)");

            migrationBuilder.AlterColumn<string>(
                name: "extension",
                table: "tbsmc_trans_attachments",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "contenttype",
                table: "tbsmc_trans_attachments",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.AddColumn<int>(
                name: "serialno",
                table: "tbsmc_trans_attachments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbsmc_transactions",
                table: "tbsmc_transactions",
                columns: new[] { "meetingid", "transactionid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbsmc_trans_attachments",
                table: "tbsmc_trans_attachments",
                columns: new[] { "rowid", "serialno" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbsmc_transactions",
                table: "tbsmc_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbsmc_trans_attachments",
                table: "tbsmc_trans_attachments");

            migrationBuilder.DropColumn(
                name: "serialno",
                table: "tbsmc_trans_attachments");

            migrationBuilder.AlterColumn<string>(
                name: "filepath",
                table: "tbsmc_trans_attachments",
                type: "varchar(500)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "extension",
                table: "tbsmc_trans_attachments",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "contenttype",
                table: "tbsmc_trans_attachments",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "transid",
                table: "tbsmc_trans_attachments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbsmc_transactions",
                table: "tbsmc_transactions",
                column: "rowid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbsmc_trans_attachments",
                table: "tbsmc_trans_attachments",
                column: "rowid");

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_transactions_meetingid",
                table: "tbsmc_transactions",
                column: "meetingid");
        }
    }
}
