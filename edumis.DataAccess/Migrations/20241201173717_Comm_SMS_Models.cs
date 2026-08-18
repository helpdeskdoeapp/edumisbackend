using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Comm_SMS_Models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbcom_sentotp",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sentto = table.Column<string>(type: "varchar(100)", nullable: false),
                    purpose = table.Column<string>(type: "varchar(100)", nullable: true),
                    ipaddress = table.Column<string>(type: "varchar(15)", nullable: true),
                    otp = table.Column<string>(type: "varchar(10)", nullable: false),
                    sentdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    validupto = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbcom_sentotp", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tbcom_sms_settings",
                columns: table => new
                {
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<string>(type: "varchar(250)", nullable: false),
                    password = table.Column<string>(type: "varchar(250)", nullable: false),
                    securekey = table.Column<string>(type: "varchar(250)", nullable: false),
                    senderid = table.Column<string>(type: "varchar(250)", nullable: false),
                    appkey = table.Column<string>(type: "varchar(250)", nullable: false),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbcom_sms_settings", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tbcom_smstemplates",
                columns: table => new
                {
                    templateid = table.Column<string>(type: "varchar(250)", nullable: false),
                    message = table.Column<string>(type: "text", nullable: true),
                    smstype = table.Column<int>(type: "integer", nullable: false),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbcom_smstemplates", x => x.templateid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbcom_sentotp");

            migrationBuilder.DropTable(
                name: "tbcom_sms_settings");

            migrationBuilder.DropTable(
                name: "tbcom_smstemplates");
        }
    }
}
