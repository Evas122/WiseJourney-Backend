using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiseJourneyBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refactor_trip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateUtc",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "StartDateUtc",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "ScheduleTimeUtc",
                table: "TripPlaces");

            migrationBuilder.DropColumn(
                name: "DateUtc",
                table: "TripDays");

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "TripDays",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "TripDays");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateUtc",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateUtc",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleTimeUtc",
                table: "TripPlaces",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUtc",
                table: "TripDays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
