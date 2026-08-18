using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class smc_budget_allocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbsmc_allocation_history",
                columns: table => new
                {
                    row_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session = table.Column<string>(type: "varchar(10)", nullable: false),
                    school_id = table.Column<string>(type: "varchar(50)", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    allocation_type = table.Column<int>(type: "integer", nullable: false),
                    allocation_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    donor_name = table.Column<string>(type: "varchar(200)", nullable: true),
                    donor_pan = table.Column<string>(type: "varchar(10)", nullable: true),
                    donor_mobile = table.Column<string>(type: "varchar(13)", nullable: true),
                    donor_address = table.Column<string>(type: "varchar(400)", nullable: true),
                    remarks = table.Column<string>(type: "varchar(400)", nullable: true),
                    created_by = table.Column<string>(type: "varchar(200)", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsmc_allocation_history", x => x.row_id);
                    table.ForeignKey(
                        name: "FK_tbsmc_allocation_history_tbglacademicsessions_session",
                        column: x => x.session,
                        principalTable: "tbglacademicsessions",
                        principalColumn: "forsession",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbsmc_allocation_history_tbmsbranches_school_id",
                        column: x => x.school_id,
                        principalTable: "tbmsbranches",
                        principalColumn: "branchid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbsmc_allocations",
                columns: table => new
                {
                    session = table.Column<string>(type: "varchar(10)", nullable: false),
                    school_id = table.Column<string>(type: "varchar(50)", nullable: false),
                    allocation = table.Column<decimal>(type: "numeric", nullable: false),
                    consumption = table.Column<decimal>(type: "numeric", nullable: false),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsmc_allocations", x => new { x.session, x.school_id });
                    table.ForeignKey(
                        name: "FK_tbsmc_allocations_tbglacademicsessions_session",
                        column: x => x.session,
                        principalTable: "tbglacademicsessions",
                        principalColumn: "forsession",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbsmc_allocations_tbmsbranches_school_id",
                        column: x => x.school_id,
                        principalTable: "tbmsbranches",
                        principalColumn: "branchid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_allocation_history_school_id",
                table: "tbsmc_allocation_history",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_allocation_history_session",
                table: "tbsmc_allocation_history",
                column: "session");

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_allocations_school_id",
                table: "tbsmc_allocations",
                column: "school_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbsmc_allocation_history");

            migrationBuilder.DropTable(
                name: "tbsmc_allocations");
        }
    }
}
