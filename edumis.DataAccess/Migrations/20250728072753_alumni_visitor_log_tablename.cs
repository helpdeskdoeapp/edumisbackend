using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class alumni_visitor_log_tablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbalm_visitor_logs",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    visitdatetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ipaddress = table.Column<string>(type: "varchar(50)", nullable: true),
                    useragent = table.Column<string>(type: "varchar(512)", nullable: true),
                    country = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbalm_visitor_logs", x => x.rowid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbalm_visitor_logs");
        }
    }
}
