using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Update_SMC_Trans_Models3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbsmc_trans_attachments",
                table: "tbsmc_trans_attachments");

            migrationBuilder.AddColumn<Guid>(
                name: "transactionid",
                table: "tbsmc_trans_attachments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbsmc_trans_attachments",
                table: "tbsmc_trans_attachments",
                columns: new[] { "transactionid", "serialno" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbsmc_trans_attachments",
                table: "tbsmc_trans_attachments");

            migrationBuilder.DropColumn(
                name: "transactionid",
                table: "tbsmc_trans_attachments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbsmc_trans_attachments",
                table: "tbsmc_trans_attachments",
                columns: new[] { "rowid", "serialno" });
        }
    }
}
