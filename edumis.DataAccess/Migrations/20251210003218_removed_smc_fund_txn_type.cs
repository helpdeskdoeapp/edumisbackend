using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removed_smc_fund_txn_type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "transtype",
                table: "tbsmc_transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "transtype",
                table: "tbsmc_transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
