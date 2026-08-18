using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class added_newspaper_magazine_apis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "awards",
                table: "tbbk_magazine");

            migrationBuilder.AddColumn<string>(
                name: "branchid",
                table: "tbbk_newspaper",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "frequency",
                table: "tbbk_newspaper",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isactive",
                table: "tbbk_newspaper",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "branchid",
                table: "tbbk_magazine",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "edition",
                table: "tbbk_magazine",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "frequency",
                table: "tbbk_magazine",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "totalqty",
                table: "tbbk_magazine",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "branchid",
                table: "tbbk_newspaper");

            migrationBuilder.DropColumn(
                name: "frequency",
                table: "tbbk_newspaper");

            migrationBuilder.DropColumn(
                name: "isactive",
                table: "tbbk_newspaper");

            migrationBuilder.DropColumn(
                name: "branchid",
                table: "tbbk_magazine");

            migrationBuilder.DropColumn(
                name: "edition",
                table: "tbbk_magazine");

            migrationBuilder.DropColumn(
                name: "frequency",
                table: "tbbk_magazine");

            migrationBuilder.DropColumn(
                name: "totalqty",
                table: "tbbk_magazine");

            migrationBuilder.AddColumn<string>(
                name: "awards",
                table: "tbbk_magazine",
                type: "text",
                nullable: true);
        }
    }
}
