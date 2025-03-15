using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTravel.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeatandBookingsmigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_BusBookings_BusBookingId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_BusBookingId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "BusBookingId",
                table: "Seats");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDate",
                table: "BusBookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SelectedSeatIds",
                table: "BusBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "SelectedSeats",
                table: "BusBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "BusBookings");

            migrationBuilder.DropColumn(
                name: "SelectedSeatIds",
                table: "BusBookings");

            migrationBuilder.DropColumn(
                name: "SelectedSeats",
                table: "BusBookings");

            migrationBuilder.AddColumn<Guid>(
                name: "BusBookingId",
                table: "Seats",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_BusBookingId",
                table: "Seats",
                column: "BusBookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_BusBookings_BusBookingId",
                table: "Seats",
                column: "BusBookingId",
                principalTable: "BusBookings",
                principalColumn: "Id");
        }
    }
}
