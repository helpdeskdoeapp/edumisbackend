using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModelUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbglcodevalues",
                table: "tbglcodevalues");

            migrationBuilder.DropIndex(
                name: "IX_tbglcodevalues_code",
                table: "tbglcodevalues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbmsemployees",
                table: "tbmsemployees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbmsempeducationdetails",
                table: "tbmsempeducationdetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbmsempappointmentdetails",
                table: "tbmsempappointmentdetails");

            migrationBuilder.RenameTable(
                name: "tbmsemployees",
                newName: "tbemp_employees");

            migrationBuilder.RenameTable(
                name: "tbmsempeducationdetails",
                newName: "tbemp_educationdetails");

            migrationBuilder.RenameTable(
                name: "tbmsempappointmentdetails",
                newName: "tbemp_appointmentdetails");

            migrationBuilder.AddColumn<string>(
                name: "emailid",
                table: "tbmslogin",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_email_verified",
                table: "tbmslogin",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InfrastructureNavigationBuildingId",
                table: "tbmsbranches",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "tbmsbranches",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "codevalue",
                table: "tbglcodevalues",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbglcodevalues",
                table: "tbglcodevalues",
                columns: new[] { "code", "codevalue" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbemp_employees",
                table: "tbemp_employees",
                column: "employeeid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbemp_educationdetails",
                table: "tbemp_educationdetails",
                columns: new[] { "employeeid", "serialno" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbemp_appointmentdetails",
                table: "tbemp_appointmentdetails",
                column: "employeeid");

            migrationBuilder.CreateTable(
                name: "IssueDetailModel",
                columns: table => new
                {
                    issueid = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueDetailModel", x => x.issueid);
                });

            migrationBuilder.CreateTable(
                name: "IssueRelatedAttachmentsModel",
                columns: table => new
                {
                    issueid = table.Column<Guid>(type: "uuid", nullable: false),
                    serialno = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "varchar(500)", nullable: true),
                    filename = table.Column<string>(type: "varchar(500)", nullable: true),
                    contenttype = table.Column<string>(type: "varchar(100)", nullable: true),
                    extension = table.Column<string>(type: "varchar(50)", nullable: true),
                    filepath = table.Column<string>(type: "varchar(500)", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueRelatedAttachmentsModel", x => new { x.issueid, x.serialno });
                    table.ForeignKey(
                        name: "FK_IssueRelatedAttachmentsModel_IssueDetailModel_issueid",
                        column: x => x.issueid,
                        principalTable: "IssueDetailModel",
                        principalColumn: "issueid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IssueRelatedCommentsModel",
                columns: table => new
                {
                    issueid = table.Column<Guid>(type: "uuid", nullable: false),
                    serialno = table.Column<int>(type: "integer", nullable: false),
                    comment_type = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueRelatedCommentsModel", x => new { x.issueid, x.serialno });
                    table.ForeignKey(
                        name: "FK_IssueRelatedCommentsModel_IssueDetailModel_issueid",
                        column: x => x.issueid,
                        principalTable: "IssueDetailModel",
                        principalColumn: "issueid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbmsbranches_InfrastructureNavigationBuildingId",
                table: "tbmsbranches",
                column: "InfrastructureNavigationBuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_tbms_zones_districtid",
                table: "tbms_zones",
                column: "districtid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbemp_appointmentdetails_tbemp_employees_employeeid",
                table: "tbemp_appointmentdetails",
                column: "employeeid",
                principalTable: "tbemp_employees",
                principalColumn: "employeeid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbemp_educationdetails_tbemp_employees_employeeid",
                table: "tbemp_educationdetails",
                column: "employeeid",
                principalTable: "tbemp_employees",
                principalColumn: "employeeid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbms_schooldetails_tbmsbranches_branchid",
                table: "tbms_schooldetails",
                column: "branchid",
                principalTable: "tbmsbranches",
                principalColumn: "branchid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbms_zones_tbms_districts_districtid",
                table: "tbms_zones",
                column: "districtid",
                principalTable: "tbms_districts",
                principalColumn: "rowid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbmsbranches_tbmsinfrastructure_InfrastructureNavigationBui~",
                table: "tbmsbranches",
                column: "InfrastructureNavigationBuildingId",
                principalTable: "tbmsinfrastructure",
                principalColumn: "buildingid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbemp_appointmentdetails_tbemp_employees_employeeid",
                table: "tbemp_appointmentdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_tbemp_educationdetails_tbemp_employees_employeeid",
                table: "tbemp_educationdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_tbms_schooldetails_tbmsbranches_branchid",
                table: "tbms_schooldetails");

            migrationBuilder.DropForeignKey(
                name: "FK_tbms_zones_tbms_districts_districtid",
                table: "tbms_zones");

            migrationBuilder.DropForeignKey(
                name: "FK_tbmsbranches_tbmsinfrastructure_InfrastructureNavigationBui~",
                table: "tbmsbranches");

            migrationBuilder.DropTable(
                name: "IssueRelatedAttachmentsModel");

            migrationBuilder.DropTable(
                name: "IssueRelatedCommentsModel");

            migrationBuilder.DropTable(
                name: "IssueDetailModel");

            migrationBuilder.DropIndex(
                name: "IX_tbmsbranches_InfrastructureNavigationBuildingId",
                table: "tbmsbranches");

            migrationBuilder.DropIndex(
                name: "IX_tbms_zones_districtid",
                table: "tbms_zones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbglcodevalues",
                table: "tbglcodevalues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbemp_employees",
                table: "tbemp_employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbemp_educationdetails",
                table: "tbemp_educationdetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbemp_appointmentdetails",
                table: "tbemp_appointmentdetails");

            migrationBuilder.DropColumn(
                name: "emailid",
                table: "tbmslogin");

            migrationBuilder.DropColumn(
                name: "is_email_verified",
                table: "tbmslogin");

            migrationBuilder.DropColumn(
                name: "InfrastructureNavigationBuildingId",
                table: "tbmsbranches");

            migrationBuilder.DropColumn(
                name: "address",
                table: "tbmsbranches");

            migrationBuilder.RenameTable(
                name: "tbemp_employees",
                newName: "tbmsemployees");

            migrationBuilder.RenameTable(
                name: "tbemp_educationdetails",
                newName: "tbmsempeducationdetails");

            migrationBuilder.RenameTable(
                name: "tbemp_appointmentdetails",
                newName: "tbmsempappointmentdetails");

            migrationBuilder.AlterColumn<int>(
                name: "codevalue",
                table: "tbglcodevalues",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbglcodevalues",
                table: "tbglcodevalues",
                column: "codevalue");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbmsemployees",
                table: "tbmsemployees",
                column: "employeeid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbmsempeducationdetails",
                table: "tbmsempeducationdetails",
                columns: new[] { "employeeid", "serialno" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbmsempappointmentdetails",
                table: "tbmsempappointmentdetails",
                column: "employeeid");

            migrationBuilder.CreateIndex(
                name: "IX_tbglcodevalues_code",
                table: "tbglcodevalues",
                column: "code");
        }
    }
}
