using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SMC_Library_model_DTO_updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "tbbk_catalogue_pkey",
                table: "tbbk_catalogue");

            migrationBuilder.AddColumn<int>(
                name: "transmode",
                table: "tbsmc_transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "accessionno",
                table: "tbbk_catalogue",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "conditionnotes",
                table: "tbbk_catalogue",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "damagetype",
                table: "tbbk_catalogue",
                type: "integer",
                nullable: true);            

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbbk_catalogue",
                table: "tbbk_catalogue",
                columns: new[] { "bookid", "accessionno" });

            migrationBuilder.CreateTable(
                name: "tbalm_alumnidetails",
                columns: table => new
                {
                    alumni_id = table.Column<Guid>(type: "uuid", nullable: false),
                    doeid = table.Column<string>(type: "varchar(50)", nullable: false),
                    salutation = table.Column<int>(type: "integer", nullable: false),
                    firstname = table.Column<string>(type: "varchar(250)", nullable: false),
                    lastname = table.Column<string>(type: "varchar(250)", nullable: true),
                    middlename = table.Column<string>(type: "varchar(250)", nullable: true),
                    dob = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    reg_year = table.Column<int>(type: "integer", nullable: false),
                    exityear = table.Column<int>(type: "integer", nullable: false),
                    branchid = table.Column<string>(type: "varchar(50)", nullable: false),
                    emailid = table.Column<string>(type: "varchar(250)", nullable: false),
                    mobileno = table.Column<string>(type: "varchar(10)", nullable: false),
                    cur_org = table.Column<string>(type: "varchar(500)", nullable: true),
                    cur_designation = table.Column<string>(type: "varchar(250)", nullable: true),
                    cur_residence = table.Column<string>(type: "varchar(500)", nullable: true),
                    residence_contactno = table.Column<string>(type: "varchar(30)", nullable: true),
                    word_contactno = table.Column<string>(type: "varchar(30)", nullable: true),
                    profile_image = table.Column<string>(type: "varchar(150)", nullable: true),
                    profile_imageextn = table.Column<string>(type: "varchar(150)", nullable: true),
                    profile_image_contenttype = table.Column<string>(type: "varchar(150)", nullable: true),
                    cur_residence_city = table.Column<string>(type: "varchar(250)", nullable: true),
                    cur_profession = table.Column<int>(type: "integer", nullable: true),
                    other_profession = table.Column<string>(type: "varchar(250)", nullable: true),
                    is_delhi_resident = table.Column<bool>(type: "boolean", nullable: false),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbalm_alumnidetails", x => x.alumni_id);
                });

            migrationBuilder.CreateTable(
                name: "tbalm_infoshare_permissions",
                columns: table => new
                {
                    alumni_id = table.Column<Guid>(type: "uuid", nullable: false),
                    emailid = table.Column<bool>(type: "boolean", nullable: false),
                    mobileno = table.Column<bool>(type: "boolean", nullable: false),
                    current_Org = table.Column<bool>(type: "boolean", nullable: false),
                    current_designation = table.Column<bool>(type: "boolean", nullable: false),
                    current_residence = table.Column<bool>(type: "boolean", nullable: false),
                    residence_contactno = table.Column<bool>(type: "boolean", nullable: false),
                    work_contactno = table.Column<bool>(type: "boolean", nullable: false),
                    current_residence_city = table.Column<bool>(type: "boolean", nullable: false),
                    current_profession = table.Column<bool>(type: "boolean", nullable: false),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbalm_infoshare_permissions", x => x.alumni_id);
                    table.ForeignKey(
                        name: "FK_tbalm_infoshare_permissions_tbalm_alumnidetails_alumni_id",
                        column: x => x.alumni_id,
                        principalTable: "tbalm_alumnidetails",
                        principalColumn: "alumni_id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbalm_infoshare_permissions");

            migrationBuilder.DropTable(
                name: "tbalm_alumnidetails");

            migrationBuilder.DropPrimaryKey(
                name: "tbbk_catalogue_pkey",
                table: "tbbk_catalogue");

            migrationBuilder.DropColumn(
                name: "transmode",
                table: "tbsmc_transactions");

            migrationBuilder.DropColumn(
                name: "conditionnotes",
                table: "tbbk_catalogue");

            migrationBuilder.DropColumn(
                name: "damagetype",
                table: "tbbk_catalogue");

            migrationBuilder.AlterColumn<int>(
                name: "accessionno",
                table: "tbbk_catalogue",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

          
            migrationBuilder.AddPrimaryKey(
                name: "PK_tbbk_catalogue",
                table: "tbbk_catalogue",
                column: "bookid");
        }
    }
}
