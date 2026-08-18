using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class alumni_refresh_token_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_tbalm_login_alumni_id",
                table: "tbalm_login",
                column: "alumni_id");

            migrationBuilder.CreateTable(
                name: "tbalm_refeshtoken",
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
                    table.PrimaryKey("PK_tbalm_refeshtoken", x => x.rowid);
                    table.ForeignKey(
                        name: "FK_tbalm_refeshtoken_tbalm_login_userid",
                        column: x => x.userid,
                        principalTable: "tbalm_login",
                        principalColumn: "alumni_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbalm_refeshtoken_token",
                table: "tbalm_refeshtoken",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbalm_refeshtoken_userid",
                table: "tbalm_refeshtoken",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbalm_refeshtoken");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_tbalm_login_alumni_id",
                table: "tbalm_login");
        }
    }
}
