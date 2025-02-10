using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyTravel.Web.Migrations
{
    /// <inheritdoc />
    public partial class addNewPhotographerAndGuide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guide_Agencies_AgencyId",
                table: "Guide");

            migrationBuilder.DropForeignKey(
                name: "FK_Photographer_Agencies_AgencyId",
                table: "Photographer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photographer",
                table: "Photographer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guide",
                table: "Guide");

            migrationBuilder.RenameTable(
                name: "Photographer",
                newName: "Photographers");

            migrationBuilder.RenameTable(
                name: "Guide",
                newName: "Guides");

            migrationBuilder.RenameIndex(
                name: "IX_Photographer_AgencyId",
                table: "Photographers",
                newName: "IX_Photographers_AgencyId");

            migrationBuilder.RenameIndex(
                name: "IX_Guide_AgencyId",
                table: "Guides",
                newName: "IX_Guides_AgencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photographers",
                table: "Photographers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guides",
                table: "Guides",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Guides_Agencies_AgencyId",
                table: "Guides",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photographers_Agencies_AgencyId",
                table: "Photographers",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guides_Agencies_AgencyId",
                table: "Guides");

            migrationBuilder.DropForeignKey(
                name: "FK_Photographers_Agencies_AgencyId",
                table: "Photographers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photographers",
                table: "Photographers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guides",
                table: "Guides");

            migrationBuilder.RenameTable(
                name: "Photographers",
                newName: "Photographer");

            migrationBuilder.RenameTable(
                name: "Guides",
                newName: "Guide");

            migrationBuilder.RenameIndex(
                name: "IX_Photographers_AgencyId",
                table: "Photographer",
                newName: "IX_Photographer_AgencyId");

            migrationBuilder.RenameIndex(
                name: "IX_Guides_AgencyId",
                table: "Guide",
                newName: "IX_Guide_AgencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photographer",
                table: "Photographer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guide",
                table: "Guide",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Guide_Agencies_AgencyId",
                table: "Guide",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photographer_Agencies_AgencyId",
                table: "Photographer",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
