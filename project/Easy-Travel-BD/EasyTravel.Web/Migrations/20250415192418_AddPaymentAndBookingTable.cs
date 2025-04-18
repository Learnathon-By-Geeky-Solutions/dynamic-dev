using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTravel.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentAndBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusBookings_AspNetUsers_UserId",
                table: "BusBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_CarBookings_AspNetUsers_UserId",
                table: "CarBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_GuideBookings_AspNetUsers_UserId",
                table: "GuideBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelBookings_AspNetUsers_UserId",
                table: "HotelBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotographerBookings_AspNetUsers_UserId",
                table: "PhotographerBookings");

            migrationBuilder.DropIndex(
                name: "IX_PhotographerBookings_UserId",
                table: "PhotographerBookings");

            migrationBuilder.DropIndex(
                name: "IX_HotelBookings_UserId",
                table: "HotelBookings");

            migrationBuilder.DropIndex(
                name: "IX_GuideBookings_UserId",
                table: "GuideBookings");

            migrationBuilder.DropIndex(
                name: "IX_CarBookings_UserId",
                table: "CarBookings");

            migrationBuilder.DropIndex(
                name: "IX_BusBookings_UserId",
                table: "BusBookings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PhotographerBookings");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "PhotographerBookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PhotographerBookings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "HotelBookings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "HotelBookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HotelBookings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "GuideBookings");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "GuideBookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GuideBookings");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "CarBookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CarBookings");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "BusBookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BusBookings");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingTypes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Guides",
                keyColumn: "Id",
                keyValue: new Guid("8a9c56d0-8be6-4d34-8e0f-8a7c0a7d9637"),
                column: "DateOfBirth",
                value: new DateTime(2025, 2, 16, 19, 31, 26, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Photographers",
                keyColumn: "Id",
                keyValue: new Guid("2a3d2f79-4f8e-4f87-8a38-41c70f4284b6"),
                column: "DateOfBirth",
                value: new DateTime(2025, 2, 16, 19, 31, 26, 0, DateTimeKind.Utc));

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                table: "Payments",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusBookings_Bookings_Id",
                table: "BusBookings",
                column: "Id",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarBookings_Bookings_Id",
                table: "CarBookings",
                column: "Id",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GuideBookings_Bookings_Id",
                table: "GuideBookings",
                column: "Id",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelBookings_Bookings_Id",
                table: "HotelBookings",
                column: "Id",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotographerBookings_Bookings_Id",
                table: "PhotographerBookings",
                column: "Id",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusBookings_Bookings_Id",
                table: "BusBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_CarBookings_Bookings_Id",
                table: "CarBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_GuideBookings_Bookings_Id",
                table: "GuideBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelBookings_Bookings_Id",
                table: "HotelBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotographerBookings_Bookings_Id",
                table: "PhotographerBookings");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PhotographerBookings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "PhotographerBookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "PhotographerBookings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "HotelBookings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "HotelBookings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "HotelBookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "GuideBookings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "GuideBookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "GuideBookings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "CarBookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CarBookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "BusBookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "BusBookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Guides",
                keyColumn: "Id",
                keyValue: new Guid("8a9c56d0-8be6-4d34-8e0f-8a7c0a7d9637"),
                column: "DateOfBirth",
                value: new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Photographers",
                keyColumn: "Id",
                keyValue: new Guid("2a3d2f79-4f8e-4f87-8a38-41c70f4284b6"),
                column: "DateOfBirth",
                value: new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_PhotographerBookings_UserId",
                table: "PhotographerBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookings_UserId",
                table: "HotelBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GuideBookings_UserId",
                table: "GuideBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarBookings_UserId",
                table: "CarBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BusBookings_UserId",
                table: "BusBookings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusBookings_AspNetUsers_UserId",
                table: "BusBookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarBookings_AspNetUsers_UserId",
                table: "CarBookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GuideBookings_AspNetUsers_UserId",
                table: "GuideBookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelBookings_AspNetUsers_UserId",
                table: "HotelBookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotographerBookings_AspNetUsers_UserId",
                table: "PhotographerBookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
