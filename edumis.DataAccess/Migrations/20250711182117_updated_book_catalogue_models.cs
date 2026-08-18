using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updated_book_catalogue_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "callno",
                table: "tbbk_catalogue");

            migrationBuilder.DropColumn(
                name: "edition",
                table: "tbbk_catalogue");

            migrationBuilder.DropColumn(
                name: "isbn",
                table: "tbbk_catalogue");

            migrationBuilder.DropColumn(
                name: "no_of_pages",
                table: "tbbk_catalogue");

            migrationBuilder.DropColumn(
                name: "publication_year",
                table: "tbbk_catalogue");

            migrationBuilder.AddColumn<string>(
                name: "callno",
                table: "tbbk_bookdetails",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "edition",
                table: "tbbk_bookdetails",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "isbn",
                table: "tbbk_bookdetails",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "no_of_pages",
                table: "tbbk_bookdetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "publication_year",
                table: "tbbk_bookdetails",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "callno",
                table: "tbbk_bookdetails");

            migrationBuilder.DropColumn(
                name: "edition",
                table: "tbbk_bookdetails");

            migrationBuilder.DropColumn(
                name: "isbn",
                table: "tbbk_bookdetails");

            migrationBuilder.DropColumn(
                name: "no_of_pages",
                table: "tbbk_bookdetails");

            migrationBuilder.DropColumn(
                name: "publication_year",
                table: "tbbk_bookdetails");

            migrationBuilder.AddColumn<string>(
                name: "callno",
                table: "tbbk_catalogue",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "edition",
                table: "tbbk_catalogue",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "isbn",
                table: "tbbk_catalogue",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "no_of_pages",
                table: "tbbk_catalogue",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "publication_year",
                table: "tbbk_catalogue",
                type: "integer",
                nullable: true);
        }
    }
}
