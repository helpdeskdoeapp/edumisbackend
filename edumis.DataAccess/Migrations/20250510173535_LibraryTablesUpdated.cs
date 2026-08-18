using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class LibraryTablesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "coverimage_contenttype",
                table: "tbbk_magazine",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "coverimage_extn",
                table: "tbbk_magazine",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "coverimage_contenttype",
                table: "tbbk_bookdetails",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "coverimage_extn",
                table: "tbbk_bookdetails",
                type: "varchar(50)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "coverimage_contenttype",
                table: "tbbk_magazine");

            migrationBuilder.DropColumn(
                name: "coverimage_extn",
                table: "tbbk_magazine");

            migrationBuilder.DropColumn(
                name: "coverimage_contenttype",
                table: "tbbk_bookdetails");

            migrationBuilder.DropColumn(
                name: "coverimage_extn",
                table: "tbbk_bookdetails");
        }
    }
}
