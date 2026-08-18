using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class leave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tble_leave_application_track",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    application_id = table.Column<string>(type: "text", nullable: false),
                    action_by = table.Column<string>(type: "text", nullable: false),
                    action_type = table.Column<string>(type: "text", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    action_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ipaddress = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tble_leave_application_track", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tble_leave_applications",
                columns: table => new
                {
                    application_id = table.Column<string>(type: "text", nullable: false, defaultValueSql: "nextval('leave_application_number')"),
                    applied_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    employee_id = table.Column<string>(type: "text", nullable: false),
                    deputed_branch_id = table.Column<string>(type: "text", nullable: true),
                    service_branch_id = table.Column<string>(type: "text", nullable: true),
                    zone_id = table.Column<string>(type: "text", nullable: true),
                    district_id = table.Column<string>(type: "text", nullable: true),
                    region_id = table.Column<string>(type: "text", nullable: true),
                    goc_id = table.Column<string>(type: "text", nullable: true),
                    hq_id = table.Column<string>(type: "text", nullable: true),
                    leave_type = table.Column<int>(type: "integer", nullable: false),
                    from_date = table.Column<DateOnly>(type: "date", nullable: false),
                    to_date = table.Column<DateOnly>(type: "date", nullable: false),
                    days = table.Column<int>(type: "integer", nullable: false),
                    address_during_leave = table.Column<string>(type: "varchar(500)", nullable: true),
                    leave_station = table.Column<string>(type: "varchar(500)", nullable: false),
                    with_noc = table.Column<bool>(type: "boolean", nullable: true),
                    child_dob = table.Column<DateOnly>(type: "date", nullable: true),
                    leave_status = table.Column<int>(type: "integer", nullable: false),
                    current_level = table.Column<int>(type: "integer", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tble_leave_applications", x => x.application_id);
                });

            migrationBuilder.CreateTable(
                name: "tble_leave_register",
                columns: table => new
                {
                    employee_id = table.Column<string>(type: "text", nullable: false),
                    cl = table.Column<float>(type: "real", nullable: false),
                    el = table.Column<int>(type: "integer", nullable: false),
                    scl = table.Column<int>(type: "integer", nullable: false),
                    pl = table.Column<int>(type: "integer", nullable: false),
                    ml = table.Column<int>(type: "integer", nullable: false),
                    hpl = table.Column<int>(type: "integer", nullable: false),
                    ccl = table.Column<int>(type: "integer", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tble_leave_register", x => x.employee_id);
                });

            migrationBuilder.CreateTable(
                name: "tble_leave_register_track",
                columns: table => new
                {
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employee_id = table.Column<string>(type: "text", nullable: false),
                    leave_type = table.Column<int>(type: "integer", nullable: false),
                    action_by = table.Column<string>(type: "text", nullable: false),
                    action_type = table.Column<string>(type: "text", nullable: false),
                    action_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    days = table.Column<float>(type: "real", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    leave_application_id = table.Column<string>(type: "text", nullable: true),
                    ipaddress = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tble_leave_register_track", x => x.rowid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tble_leave_application_track");

            migrationBuilder.DropTable(
                name: "tble_leave_applications");

            migrationBuilder.DropTable(
                name: "tble_leave_register");

            migrationBuilder.DropTable(
                name: "tble_leave_register_track");
        }
    }
}
