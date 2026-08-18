using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class alumni_profile_col_removed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cur_org",
                table: "tbalm_alumnidetails");

            migrationBuilder.RenameColumn(
                name: "word_contactno",
                table: "tbalm_alumnidetails",
                newName: "work_contactno");

            migrationBuilder.RenameColumn(
                name: "reg_year",
                table: "tbalm_alumnidetails",
                newName: "registration_year");

            migrationBuilder.RenameColumn(
                name: "profile_imageextn",
                table: "tbalm_alumnidetails",
                newName: "profile_image_extn");

            migrationBuilder.RenameColumn(
                name: "exityear",
                table: "tbalm_alumnidetails",
                newName: "exit_year");

            migrationBuilder.RenameColumn(
                name: "cur_residence_city",
                table: "tbalm_alumnidetails",
                newName: "current_Residence_city");

            migrationBuilder.RenameColumn(
                name: "cur_residence",
                table: "tbalm_alumnidetails",
                newName: "current_residence");

            migrationBuilder.RenameColumn(
                name: "cur_profession",
                table: "tbalm_alumnidetails",
                newName: "current_profession");

            migrationBuilder.RenameColumn(
                name: "cur_designation",
                table: "tbalm_alumnidetails",
                newName: "current_desig");

            migrationBuilder.RenameColumn(
                name: "profile_image",
                table: "tbalm_alumnidetails",
                newName: "altemailid");

            migrationBuilder.RenameColumn(
                name: "is_delhi_resident",
                table: "tbalm_alumnidetails",
                newName: "isactive");

            migrationBuilder.AlterColumn<string>(
                name: "profile_image_contenttype",
                table: "tbalm_alumnidetails",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "other_profession",
                table: "tbalm_alumnidetails",
                type: "varchar(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobileno",
                table: "tbalm_alumnidetails",
                type: "varchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)");

            migrationBuilder.AlterColumn<string>(
                name: "middlename",
                table: "tbalm_alumnidetails",
                type: "varchar(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "lastname",
                table: "tbalm_alumnidetails",
                type: "varchar(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "firstname",
                table: "tbalm_alumnidetails",
                type: "varchar(150)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)");

            migrationBuilder.AlterColumn<string>(
                name: "emailid",
                table: "tbalm_alumnidetails",
                type: "varchar(150)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)");

            migrationBuilder.AlterColumn<string>(
                name: "doeid",
                table: "tbalm_alumnidetails",
                type: "varchar(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "branchid",
                table: "tbalm_alumnidetails",
                type: "varchar(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "profile_image_extn",
                table: "tbalm_alumnidetails",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "current_Residence_city",
                table: "tbalm_alumnidetails",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "current_residence",
                table: "tbalm_alumnidetails",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "current_profession",
                table: "tbalm_alumnidetails",
                type: "varchar(150)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "branch_not_in_list",
                table: "tbalm_alumnidetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "current_organization",
                table: "tbalm_alumnidetails",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_email_verified",
                table: "tbalm_alumnidetails",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_resident_of_delhi",
                table: "tbalm_alumnidetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "other_branch_name",
                table: "tbalm_alumnidetails",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbalm_login",
                columns: table => new
                {
                    emailid = table.Column<string>(type: "varchar(150)", nullable: false),
                    alumni_id = table.Column<Guid>(type: "uuid", nullable: false),
                    password = table.Column<string>(type: "varchar(150)", nullable: false),
                    prevpassword1 = table.Column<string>(type: "varchar(150)", nullable: true),
                    prevpassword2 = table.Column<string>(type: "varchar(150)", nullable: true),
                    lastpwdchangeddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ispwdchangewarningsent = table.Column<bool>(type: "boolean", nullable: true),
                    maxnoofinvalidloginattempt = table.Column<int>(type: "integer", nullable: true),
                    isaccountlocked = table.Column<bool>(type: "boolean", nullable: true),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    isloggedin = table.Column<bool>(type: "boolean", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbalm_login", x => x.emailid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbalm_alumnidetails_emailid",
                table: "tbalm_alumnidetails",
                column: "emailid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbalm_login_emailid",
                table: "tbalm_login",
                column: "emailid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbalm_login");

            migrationBuilder.DropIndex(
                name: "IX_tbalm_alumnidetails_emailid",
                table: "tbalm_alumnidetails");

            migrationBuilder.DropColumn(
                name: "branch_not_in_list",
                table: "tbalm_alumnidetails");

            migrationBuilder.DropColumn(
                name: "current_organization",
                table: "tbalm_alumnidetails");

            migrationBuilder.DropColumn(
                name: "is_email_verified",
                table: "tbalm_alumnidetails");

            migrationBuilder.DropColumn(
                name: "is_resident_of_delhi",
                table: "tbalm_alumnidetails");

            migrationBuilder.DropColumn(
                name: "other_branch_name",
                table: "tbalm_alumnidetails");

            migrationBuilder.RenameColumn(
                name: "work_contactno",
                table: "tbalm_alumnidetails",
                newName: "word_contactno");

            migrationBuilder.RenameColumn(
                name: "registration_year",
                table: "tbalm_alumnidetails",
                newName: "reg_year");

            migrationBuilder.RenameColumn(
                name: "profile_image_extn",
                table: "tbalm_alumnidetails",
                newName: "profile_imageextn");

            migrationBuilder.RenameColumn(
                name: "exit_year",
                table: "tbalm_alumnidetails",
                newName: "exityear");

            migrationBuilder.RenameColumn(
                name: "current_residence",
                table: "tbalm_alumnidetails",
                newName: "cur_residence");

            migrationBuilder.RenameColumn(
                name: "current_profession",
                table: "tbalm_alumnidetails",
                newName: "cur_profession");

            migrationBuilder.RenameColumn(
                name: "current_desig",
                table: "tbalm_alumnidetails",
                newName: "cur_designation");

            migrationBuilder.RenameColumn(
                name: "current_Residence_city",
                table: "tbalm_alumnidetails",
                newName: "cur_residence_city");

            migrationBuilder.RenameColumn(
                name: "isactive",
                table: "tbalm_alumnidetails",
                newName: "is_delhi_resident");

            migrationBuilder.RenameColumn(
                name: "altemailid",
                table: "tbalm_alumnidetails",
                newName: "profile_image");

            migrationBuilder.AlterColumn<string>(
                name: "profile_image_contenttype",
                table: "tbalm_alumnidetails",
                type: "varchar(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "other_profession",
                table: "tbalm_alumnidetails",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobileno",
                table: "tbalm_alumnidetails",
                type: "varchar(10)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "middlename",
                table: "tbalm_alumnidetails",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "lastname",
                table: "tbalm_alumnidetails",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "firstname",
                table: "tbalm_alumnidetails",
                type: "varchar(250)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)");

            migrationBuilder.AlterColumn<string>(
                name: "emailid",
                table: "tbalm_alumnidetails",
                type: "varchar(250)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)");

            migrationBuilder.AlterColumn<string>(
                name: "doeid",
                table: "tbalm_alumnidetails",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "branchid",
                table: "tbalm_alumnidetails",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "profile_imageextn",
                table: "tbalm_alumnidetails",
                type: "varchar(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cur_residence",
                table: "tbalm_alumnidetails",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "cur_profession",
                table: "tbalm_alumnidetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cur_residence_city",
                table: "tbalm_alumnidetails",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cur_org",
                table: "tbalm_alumnidetails",
                type: "varchar(500)",
                nullable: true);
        }
    }
}
