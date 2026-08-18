using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class events_model_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "eventtime",
                table: "tbweb_events",
                newName: "starttime");

            migrationBuilder.RenameColumn(
                name: "eventdate",
                table: "tbweb_events",
                newName: "startdate");

            migrationBuilder.AddColumn<int>(
                name: "category",
                table: "tbweb_events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "enddate",
                table: "tbweb_events",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "endtime",
                table: "tbweb_events",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category",
                table: "tbweb_events");

            migrationBuilder.DropColumn(
                name: "enddate",
                table: "tbweb_events");

            migrationBuilder.DropColumn(
                name: "endtime",
                table: "tbweb_events");

            migrationBuilder.RenameColumn(
                name: "starttime",
                table: "tbweb_events",
                newName: "eventtime");

            migrationBuilder.RenameColumn(
                name: "startdate",
                table: "tbweb_events",
                newName: "eventdate");
        }
    }
}
