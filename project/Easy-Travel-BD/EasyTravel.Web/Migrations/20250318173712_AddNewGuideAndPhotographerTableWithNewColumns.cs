using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTravel.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddNewGuideAndPhotographerTableWithNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreferredEvents",
                table: "Photographers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredLocations",
                table: "Photographers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredLocations",
                table: "Guides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Guides",
                keyColumn: "Id",
                keyValue: new Guid("8a9c56d0-8be6-4d34-8e0f-8a7c0a7d9637"),
                column: "PreferredLocations",
                value: "dhaka,sylhet");

            migrationBuilder.UpdateData(
                table: "Photographers",
                keyColumn: "Id",
                keyValue: new Guid("2a3d2f79-4f8e-4f87-8a38-41c70f4284b6"),
                columns: new[] { "PreferredEvents", "PreferredLocations" },
                values: new object[] { "marriage", "dhaka,sylhet" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredEvents",
                table: "Photographers");

            migrationBuilder.DropColumn(
                name: "PreferredLocations",
                table: "Photographers");

            migrationBuilder.DropColumn(
                name: "PreferredLocations",
                table: "Guides");
        }
    }
}
