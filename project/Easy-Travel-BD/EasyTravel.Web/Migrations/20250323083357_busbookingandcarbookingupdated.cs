using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTravel.Web.Migrations
{
    /// <inheritdoc />
    public partial class busbookingandcarbookingupdated : Migration
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

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "BusBookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CarBookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassengerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarBookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarBookings_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusBookings_UserId",
                table: "BusBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarBookings_CarId",
                table: "CarBookings",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarBookings_UserId",
                table: "CarBookings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusBookings_AspNetUsers_UserId",
                table: "BusBookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusBookings_AspNetUsers_UserId",
                table: "BusBookings");

            migrationBuilder.DropTable(
                name: "CarBookings");

            migrationBuilder.DropIndex(
                name: "IX_BusBookings_UserId",
                table: "BusBookings");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "BusBookings");

            migrationBuilder.DropColumn(
                name: "SelectedSeatIds",
                table: "BusBookings");

            migrationBuilder.DropColumn(
                name: "SelectedSeats",
                table: "BusBookings");

            migrationBuilder.DropColumn(
                name: "UserId",
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
