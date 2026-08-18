using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class alumni_tables_updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "tbalm_alumnidetails",
                type: "varchar(120)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "show_on_home_page",
                table: "tbalm_alumnidetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "tbalm_alumnidetails");

            migrationBuilder.DropColumn(
                name: "show_on_home_page",
                table: "tbalm_alumnidetails");
        }
    }
}
