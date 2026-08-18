using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class srtt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbsmc_refeshtoken",
                columns: table => new
                {
                    rowid = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    expireson_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsmc_refeshtoken", x => x.rowid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_refeshtoken_token",
                table: "tbsmc_refeshtoken",
                column: "token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbsmc_refeshtoken");
        }
    }
}
