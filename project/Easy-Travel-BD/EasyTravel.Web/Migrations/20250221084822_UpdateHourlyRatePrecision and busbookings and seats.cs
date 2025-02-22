using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTravel.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHourlyRatePrecisionandbusbookingsandseats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusBooking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassengerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusBooking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusBooking_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    BusBookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seat_BusBooking_BusBookingId",
                        column: x => x.BusBookingId,
                        principalTable: "BusBooking",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Seat_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusBooking_BusId",
                table: "BusBooking",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_BusBookingId",
                table: "Seat",
                column: "BusBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_BusId",
                table: "Seat",
                column: "BusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "BusBooking");
        }
    }
}
