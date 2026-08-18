using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updated_table_names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberDeviceTokensModel",
                table: "MemberDeviceTokensModel");

            migrationBuilder.RenameTable(
                name: "MemberDeviceTokensModel",
                newName: "tbsmc_devicetokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbsmc_devicetokens",
                table: "tbsmc_devicetokens",
                columns: new[] { "memberid", "serialno" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbsmc_devicetokens",
                table: "tbsmc_devicetokens");

            migrationBuilder.RenameTable(
                name: "tbsmc_devicetokens",
                newName: "MemberDeviceTokensModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberDeviceTokensModel",
                table: "MemberDeviceTokensModel",
                columns: new[] { "memberid", "serialno" });
        }
    }
}
