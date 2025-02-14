using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTravel.Web.Migrations
{
    /// <inheritdoc />
    public partial class newAgencyPhotographerGuidetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Guides",
                keyColumn: "Id",
                keyValue: new Guid("8a9c56d0-8be6-4d34-8e0f-8a7c0a7d9637"),
                columns: new[] { "Email", "FirstName", "LastName" },
                values: new object[] { "irfan.mahmud@example.com", "Irfan", "Mahmud" });

            migrationBuilder.UpdateData(
                table: "Photographers",
                keyColumn: "Id",
                keyValue: new Guid("2a3d2f79-4f8e-4f87-8a38-41c70f4284b6"),
                columns: new[] { "Email", "FirstName", "LastName" },
                values: new object[] { "irfan.mahmud@example.com", "Irfan", "Mahmud" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Guides",
                keyColumn: "Id",
                keyValue: new Guid("8a9c56d0-8be6-4d34-8e0f-8a7c0a7d9637"),
                columns: new[] { "Email", "FirstName", "LastName" },
                values: new object[] { "max.doe@example.com", "Max", "Doe" });

            migrationBuilder.UpdateData(
                table: "Photographers",
                keyColumn: "Id",
                keyValue: new Guid("2a3d2f79-4f8e-4f87-8a38-41c70f4284b6"),
                columns: new[] { "Email", "FirstName", "LastName" },
                values: new object[] { "john.doe@example.com", "John", "Doe" });
        }
    }
}
