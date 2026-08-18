using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RefeshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbgluseractivitylogs",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    secondaryid = table.Column<string>(type: "varchar(250)", nullable: true),
                    activity = table.Column<string>(type: "varchar(250)", nullable: false),
                    activitydatetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ipaddress = table.Column<string>(type: "varchar(30)", nullable: false),
                    useragent = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbgluseractivitylogs", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tbms_refeshtoken",
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
                    table.PrimaryKey("PK_tbms_refeshtoken", x => x.rowid);
                    table.ForeignKey(
                        name: "FK_tbms_refeshtoken_tbmslogin_userid",
                        column: x => x.userid,
                        principalTable: "tbmslogin",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbms_refeshtoken_token",
                table: "tbms_refeshtoken",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbms_refeshtoken_userid",
                table: "tbms_refeshtoken",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbgluseractivitylogs");

            migrationBuilder.DropTable(
                name: "tbms_refeshtoken");
        }
    }
}
