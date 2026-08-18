using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class exception_log_Model_updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "stacktrace",
                table: "tbglexceptionlogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "origin",
                table: "tbglexceptionlogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "innermessage",
                table: "tbglexceptionlogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)");

            migrationBuilder.AlterColumn<string>(
                name: "errormessage",
                table: "tbglexceptionlogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "stacktrace",
                table: "tbglexceptionlogs",
                type: "varchar(4000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "origin",
                table: "tbglexceptionlogs",
                type: "varchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "innermessage",
                table: "tbglexceptionlogs",
                type: "varchar(4000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "errormessage",
                table: "tbglexceptionlogs",
                type: "varchar(4000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
