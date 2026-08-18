using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updated_news_event_tender_circular_apis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbmsnews",
                table: "tbmsnews");

            migrationBuilder.DropColumn(
                name: "filelink",
                table: "tbweb_circulars");

            migrationBuilder.RenameTable(
                name: "tbmsnews",
                newName: "tbweb_news");

            migrationBuilder.RenameColumn(
                name: "filelink",
                table: "tbweb_tenders",
                newName: "filepath");

            migrationBuilder.RenameColumn(
                name: "weblink",
                table: "tbweb_circulars",
                newName: "filepath");

            migrationBuilder.RenameColumn(
                name: "photo",
                table: "tbweb_news",
                newName: "banner_filepath");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "expirytime",
                table: "tbweb_tenders",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "file_content_Type",
                table: "tbweb_tenders",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "file_extn",
                table: "tbweb_tenders",
                type: "varchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "filename",
                table: "tbweb_tenders",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "file_content_Type",
                table: "tbweb_circulars",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "file_extn",
                table: "tbweb_circulars",
                type: "varchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "filename",
                table: "tbweb_circulars",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "videolink",
                table: "tbweb_news",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "externallink",
                table: "tbweb_news",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "alumni_news",
                table: "tbweb_news",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "banner_file_content_Type",
                table: "tbweb_news",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "banner_file_extn",
                table: "tbweb_news",
                type: "varchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "banner_filename",
                table: "tbweb_news",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "financialyear",
                table: "tbweb_news",
                type: "varchar(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isvalid",
                table: "tbweb_news",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbweb_news",
                table: "tbweb_news",
                column: "rowid");

            migrationBuilder.CreateTable(
                name: "tbweb_events",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    financialyear = table.Column<string>(type: "varchar(10)", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    venue = table.Column<string>(type: "text", nullable: false),
                    eventdate = table.Column<DateOnly>(type: "date", nullable: false),
                    eventtime = table.Column<TimeOnly>(type: "time", nullable: false),
                    organizedby = table.Column<string>(type: "varchar(500)", nullable: true),
                    branchid = table.Column<string>(type: "varchar(50)", nullable: true),
                    videolink = table.Column<string>(type: "text", nullable: true),
                    externallink = table.Column<string>(type: "text", nullable: true),
                    banner_filepath = table.Column<string>(type: "varchar(500)", nullable: true),
                    banner_filename = table.Column<string>(type: "varchar(100)", nullable: true),
                    banner_file_extn = table.Column<string>(type: "varchar(30)", nullable: true),
                    banner_file_content_Type = table.Column<string>(type: "varchar(50)", nullable: true),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    alumni_event = table.Column<bool>(type: "boolean", nullable: true),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbweb_events", x => x.rowid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbweb_events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbweb_news",
                table: "tbweb_news");

            migrationBuilder.DropColumn(
                name: "expirytime",
                table: "tbweb_tenders");

            migrationBuilder.DropColumn(
                name: "file_content_Type",
                table: "tbweb_tenders");

            migrationBuilder.DropColumn(
                name: "file_extn",
                table: "tbweb_tenders");

            migrationBuilder.DropColumn(
                name: "filename",
                table: "tbweb_tenders");

            migrationBuilder.DropColumn(
                name: "file_content_Type",
                table: "tbweb_circulars");

            migrationBuilder.DropColumn(
                name: "file_extn",
                table: "tbweb_circulars");

            migrationBuilder.DropColumn(
                name: "filename",
                table: "tbweb_circulars");

            migrationBuilder.DropColumn(
                name: "alumni_news",
                table: "tbweb_news");

            migrationBuilder.DropColumn(
                name: "banner_file_content_Type",
                table: "tbweb_news");

            migrationBuilder.DropColumn(
                name: "banner_file_extn",
                table: "tbweb_news");

            migrationBuilder.DropColumn(
                name: "banner_filename",
                table: "tbweb_news");

            migrationBuilder.DropColumn(
                name: "financialyear",
                table: "tbweb_news");

            migrationBuilder.DropColumn(
                name: "isvalid",
                table: "tbweb_news");

            migrationBuilder.RenameTable(
                name: "tbweb_news",
                newName: "tbmsnews");

            migrationBuilder.RenameColumn(
                name: "filepath",
                table: "tbweb_tenders",
                newName: "filelink");

            migrationBuilder.RenameColumn(
                name: "filepath",
                table: "tbweb_circulars",
                newName: "weblink");

            migrationBuilder.RenameColumn(
                name: "banner_filepath",
                table: "tbmsnews",
                newName: "photo");

            migrationBuilder.AddColumn<string>(
                name: "filelink",
                table: "tbweb_circulars",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "videolink",
                table: "tbmsnews",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "externallink",
                table: "tbmsnews",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbmsnews",
                table: "tbmsnews",
                column: "rowid");
        }
    }
}
