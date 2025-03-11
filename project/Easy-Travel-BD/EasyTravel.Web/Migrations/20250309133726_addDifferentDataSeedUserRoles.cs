using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EasyTravel.Web.Migrations
{
    /// <inheritdoc />
    public partial class addDifferentDataSeedUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("292dcaf2-aadc-493a-8f19-e7905ab98299"), null, "hotelManager", "hotelManager" },
                    { new Guid("4558e034-03af-4d30-819f-9a24cb81c942"), null, "agencyManager", "agencyManager" },
                    { new Guid("862b8016-7786-4cf2-bcb1-a4aac017ff2c"), null, "carManager", "carManager" },
                    { new Guid("c10f83b0-9008-468b-b931-5e73ff416337"), null, "busManager", "busManager" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                keyValue: new Guid("c10f83b0-9008-468b-b931-5e73ff416337"));
        }
    }
}
