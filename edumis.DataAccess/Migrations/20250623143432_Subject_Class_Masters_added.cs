using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Subject_Class_Masters_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbms_classes",
                columns: table => new
                {
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "varchar(150)", nullable: false),
                    class_code = table.Column<string>(type: "varchar(50)", nullable: true),
                    sections = table.Column<int[]>(type: "integer[]", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbms_classes", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tbms_subjects",
                columns: table => new
                {
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "varchar(150)", nullable: false),
                    subject_code = table.Column<string>(type: "varchar(50)", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbms_subjects", x => x.rowid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbms_classes");

            migrationBuilder.DropTable(
                name: "tbms_subjects");
        }
    }
}
