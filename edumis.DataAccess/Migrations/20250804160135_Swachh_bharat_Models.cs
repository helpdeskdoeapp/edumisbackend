using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Swachh_bharat_Models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbmisc_swachhbharatimages",
                columns: table => new
                {
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    branchid = table.Column<string>(type: "varchar(50)", nullable: false),
                    fordate = table.Column<DateOnly>(type: "date", nullable: false),
                    imageurl = table.Column<string>(type: "varchar(500)", nullable: false),
                    imagename = table.Column<string>(type: "varchar(500)", nullable: false),
                    image_contenttype = table.Column<string>(type: "varchar(50)", nullable: true),
                    image_extn = table.Column<string>(type: "varchar(20)", nullable: true),
                    iscurrent = table.Column<bool>(type: "boolean", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbmisc_swachhbharatimages", x => x.rowid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbmisc_swachhbharatimages");
        }
    }
}
