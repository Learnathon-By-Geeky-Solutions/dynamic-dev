using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EasyTravel.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotographerBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("292dcaf2-aadc-493a-8f19-e7905ab98299"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4558e034-03af-4d30-819f-9a24cb81c942"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("862b8016-7786-4cf2-bcb1-a4aac017ff2c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b3c9d8f4-1a2b-4c5d-9e6f-7a8b9c0d1e2f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c10f83b0-9008-468b-b931-5e73ff416337"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f7e6d5c4-b3a2-1f0e-9d8c-7b6a5c4d3e2f"));

            migrationBuilder.CreateTable(
                name: "PhotographerBooking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TotalTime = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotographerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotographerBooking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotographerBooking_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotographerBooking_Photographers_PhotographerId",
                        column: x => x.PhotographerId,
                        principalTable: "Photographers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhotographerBooking_PhotographerId",
                table: "PhotographerBooking",
                column: "PhotographerId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotographerBooking_UserId",
                table: "PhotographerBooking",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhotographerBooking");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("292dcaf2-aadc-493a-8f19-e7905ab98299"), null, "hotelManager", "hotelManager" },
                    { new Guid("4558e034-03af-4d30-819f-9a24cb81c942"), null, "agencyManager", "agencyManager" },
                    { new Guid("862b8016-7786-4cf2-bcb1-a4aac017ff2c"), null, "carManager", "carManager" },
                    { new Guid("b3c9d8f4-1a2b-4c5d-9e6f-7a8b9c0d1e2f"), null, "admin", "admin" },
                    { new Guid("c10f83b0-9008-468b-b931-5e73ff416337"), null, "busManager", "busManager" },
                    { new Guid("f7e6d5c4-b3a2-1f0e-9d8c-7b6a5c4d3e2f"), null, "client", "client" }
                });
        }
    }
}
