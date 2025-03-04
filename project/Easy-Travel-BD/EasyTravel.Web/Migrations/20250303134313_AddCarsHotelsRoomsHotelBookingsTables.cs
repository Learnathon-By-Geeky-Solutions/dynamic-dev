using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTravel.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCarsHotelsRoomsHotelBookingsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelBooking_AspNetUsers_UserId",
                table: "HotelBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelBooking_Hotels_HotelId",
                table: "HotelBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Hotels_HotelId",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelBooking",
                table: "HotelBooking");

            migrationBuilder.RenameTable(
                name: "Room",
                newName: "Rooms");

            migrationBuilder.RenameTable(
                name: "HotelBooking",
                newName: "HotelBookings");

            migrationBuilder.RenameIndex(
                name: "IX_Room_HotelId",
                table: "Rooms",
                newName: "IX_Rooms_HotelId");

            migrationBuilder.RenameIndex(
                name: "IX_HotelBooking_UserId",
                table: "HotelBookings",
                newName: "IX_HotelBookings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_HotelBooking_HotelId",
                table: "HotelBookings",
                newName: "IX_HotelBookings_HotelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelBookings",
                table: "HotelBookings",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperatorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CarType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    From = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    To = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_HotelBookings_AspNetUsers_UserId",
                table: "HotelBookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelBookings_Hotels_HotelId",
                table: "HotelBookings",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Hotels_HotelId",
                table: "Rooms",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelBookings_AspNetUsers_UserId",
                table: "HotelBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelBookings_Hotels_HotelId",
                table: "HotelBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Hotels_HotelId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelBookings",
                table: "HotelBookings");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Room");

            migrationBuilder.RenameTable(
                name: "HotelBookings",
                newName: "HotelBooking");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_HotelId",
                table: "Room",
                newName: "IX_Room_HotelId");

            migrationBuilder.RenameIndex(
                name: "IX_HotelBookings_UserId",
                table: "HotelBooking",
                newName: "IX_HotelBooking_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_HotelBookings_HotelId",
                table: "HotelBooking",
                newName: "IX_HotelBooking_HotelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelBooking",
                table: "HotelBooking",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelBooking_AspNetUsers_UserId",
                table: "HotelBooking",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelBooking_Hotels_HotelId",
                table: "HotelBooking",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Hotels_HotelId",
                table: "Room",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
