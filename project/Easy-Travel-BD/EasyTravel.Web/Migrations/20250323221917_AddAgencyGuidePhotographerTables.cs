﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTravel.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAgencyGuidePhotographerTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Guides",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LanguagesSpoken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PreferredLocations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredEvents = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "Active"),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guides_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photographers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredLocations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredEvents = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortfolioUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SocialMediaLinks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "Active"),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photographers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photographers_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Agencies",
                columns: new[] { "Id", "Address", "ContactNumber", "LicenseNumber", "Name", "Website" },
                values: new object[] { new Guid("b8a1d0c5-3f2b-4f8a-9d87-1e4f2e6c1a5b"), "Mirpur", "01723695124", "365981", "Irfan", "irfan.com" });

            migrationBuilder.InsertData(
                table: "Guides",
                columns: new[] { "Id", "Address", "AgencyId", "Availability", "Bio", "ContactNumber", "DateOfBirth", "Email", "FirstName", "HourlyRate", "LanguagesSpoken", "LastName", "LicenseNumber", "PreferredEvents", "PreferredLocations", "ProfilePicture", "Rating", "Specialization", "Status", "UpdatedAt", "YearsOfExperience" },
                values: new object[] { new Guid("8a9c56d0-8be6-4d34-8e0f-8a7c0a7d9637"), "1234 Main St, City, Country", new Guid("b8a1d0c5-3f2b-4f8a-9d87-1e4f2e6c1a5b"), true, "Experienced professional with a background in various fields.", "+1234567890", new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "irfan.mahmud@example.com", "Irfan", 50.00m, "English, Spanish", "Mahmud", "ABC123456", "citytour,museumtour,hilltracking", "dhaka,sylhet", "profile-pic.jpg", 4.5m, "Communication,Hiking,Swimming,Skydive", "Active", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 });

            migrationBuilder.InsertData(
                table: "Photographers",
                columns: new[] { "Id", "Address", "AgencyId", "Availability", "Bio", "ContactNumber", "DateOfBirth", "Email", "FirstName", "HourlyRate", "LastName", "PortfolioUrl", "PreferredEvents", "PreferredLocations", "ProfilePicture", "Rating", "Skills", "SocialMediaLinks", "Specialization", "Status", "UpdatedAt", "YearsOfExperience" },
                values: new object[] { new Guid("2a3d2f79-4f8e-4f87-8a38-41c70f4284b6"), "123 Main St, Anytown, USA", new Guid("b8a1d0c5-3f2b-4f8a-9d87-1e4f2e6c1a5b"), true, "Experienced photographer with a passion for Nature photography.", "123-456-7890", new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "irfan.mahmud@example.com", "Irfan", 50.00m, "Mahmud", "https://johndoeportfolio.com", "marriage", "dhaka,sylhet", "profile.jpg", 4.5m, "Photography,Video Editing,Grahphics Design", "https://twitter.com/johndoe", "Communication,Hiking,Swimming,Skydive", "Active", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 });

            migrationBuilder.CreateIndex(
                name: "IX_Guides_AgencyId",
                table: "Guides",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Photographers_AgencyId",
                table: "Photographers",
                column: "AgencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guides");

            migrationBuilder.DropTable(
                name: "Photographers");

            migrationBuilder.DropTable(
                name: "Agencies");
        }
    }
}
