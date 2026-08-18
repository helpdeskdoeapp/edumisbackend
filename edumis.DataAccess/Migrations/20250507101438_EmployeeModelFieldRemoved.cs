using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeModelFieldRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "selectioncategory",
                table: "tbemp_employees");

            migrationBuilder.AddColumn<int>(
                name: "selectioncategory",
                table: "tbemp_appointmentdetails",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "selectioncategory",
                table: "tbemp_appointmentdetails");

            migrationBuilder.AddColumn<int>(
                name: "selectioncategory",
                table: "tbemp_employees",
                type: "integer",
                nullable: true);
        }
    }
}
