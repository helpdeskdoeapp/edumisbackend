using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class modify_visitor_log_tablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VisitorCounter",
                table: "VisitorCounter");

            migrationBuilder.RenameTable(
                name: "VisitorCounter",
                newName: "tbgl_visitor_logs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbgl_visitor_logs",
                table: "tbgl_visitor_logs",
                column: "rowid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbgl_visitor_logs",
                table: "tbgl_visitor_logs");

            migrationBuilder.RenameTable(
                name: "tbgl_visitor_logs",
                newName: "VisitorCounter");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VisitorCounter",
                table: "VisitorCounter",
                column: "rowid");
        }
    }
}
