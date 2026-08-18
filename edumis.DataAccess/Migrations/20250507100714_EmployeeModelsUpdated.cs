using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeModelsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contenttype",
                table: "tbemp_employees",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "extension",
                table: "tbemp_employees",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "photo",
                table: "tbemp_employees",
                type: "bytea",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbemp_achievements",
                columns: table => new
                {
                    employeeid = table.Column<string>(type: "varchar(50)", nullable: false),
                    serialno = table.Column<int>(type: "integer", nullable: false),
                    achievement = table.Column<string>(type: "text", nullable: false),
                    fileuploaded = table.Column<string>(type: "varchar(250)", nullable: true),
                    fileextension = table.Column<string>(type: "varchar(50)", nullable: true),
                    filecontenttype = table.Column<string>(type: "varchar(100)", nullable: true),
                    filepath = table.Column<string>(type: "varchar(250)", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbemp_achievements", x => new { x.employeeid, x.serialno });
                    table.ForeignKey(
                        name: "FK_tbemp_achievements_tbemp_employees_employeeid",
                        column: x => x.employeeid,
                        principalTable: "tbemp_employees",
                        principalColumn: "employeeid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbemp_experiences",
                columns: table => new
                {
                    employeeid = table.Column<string>(type: "varchar(50)", nullable: false),
                    serialno = table.Column<int>(type: "integer", nullable: false),
                    experience = table.Column<string>(type: "text", nullable: false),
                    fileuploaded = table.Column<string>(type: "varchar(250)", nullable: true),
                    fileextension = table.Column<string>(type: "varchar(50)", nullable: true),
                    filecontenttype = table.Column<string>(type: "varchar(100)", nullable: true),
                    filepath = table.Column<string>(type: "varchar(250)", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbemp_experiences", x => new { x.employeeid, x.serialno });
                    table.ForeignKey(
                        name: "FK_tbemp_experiences_tbemp_employees_employeeid",
                        column: x => x.employeeid,
                        principalTable: "tbemp_employees",
                        principalColumn: "employeeid",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbemp_achievements");

            migrationBuilder.DropTable(
                name: "tbemp_experiences");

            migrationBuilder.DropColumn(
                name: "contenttype",
                table: "tbemp_employees");

            migrationBuilder.DropColumn(
                name: "extension",
                table: "tbemp_employees");

            migrationBuilder.DropColumn(
                name: "photo",
                table: "tbemp_employees");
        }
    }
}
