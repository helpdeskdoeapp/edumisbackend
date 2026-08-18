using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_school_details_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbms_schooldetails",
                columns: table => new
                {
                    branchid = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    udisecode = table.Column<string>(type: "varchar(50)", nullable: true),
                    shift = table.Column<int>(type: "integer", nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: true),
                    estbyear = table.Column<int>(type: "integer", nullable: true),
                    nomenclature = table.Column<int>(type: "integer", nullable: true),
                    policestation = table.Column<string>(type: "text", nullable: true),
                    hospital = table.Column<string>(type: "varchar(250)", nullable: true),
                    assembly = table.Column<string>(type: "varchar(150)", nullable: true),
                    constituency = table.Column<string>(type: "varchar(50)", nullable: true),
                    streams = table.Column<int[]>(type: "integer[]", nullable: true),
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbms_schooldetails", x => x.branchid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbms_schooldetails");
        }
    }
}
