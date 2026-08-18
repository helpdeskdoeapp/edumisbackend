using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updated_sms_admin_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uniqueid",
                table: "tbsmc_logins");

            migrationBuilder.AlterColumn<bool>(
                name: "isvalid",
                table: "tbsmc_logins",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "emailid",
                table: "tbsmc_logins",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mobileno",
                table: "tbsmc_logins",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "inchargeid",
                table: "tbmsbranches",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "emailid",
                table: "tbsmc_logins");

            migrationBuilder.DropColumn(
                name: "mobileno",
                table: "tbsmc_logins");

            migrationBuilder.AlterColumn<bool>(
                name: "isvalid",
                table: "tbsmc_logins",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uniqueid",
                table: "tbsmc_logins",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "inchargeid",
                table: "tbmsbranches",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);
        }
    }
}
