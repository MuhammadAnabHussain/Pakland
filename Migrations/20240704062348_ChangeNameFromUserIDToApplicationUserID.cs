using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pakland.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameFromUserIDToApplicationUserID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDetails_AspNetUsers_UserId",
                table: "PropertyDetails");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PropertyDetails",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyDetails_UserId",
                table: "PropertyDetails",
                newName: "IX_PropertyDetails_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDetails_AspNetUsers_ApplicationUserId",
                table: "PropertyDetails",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDetails_AspNetUsers_ApplicationUserId",
                table: "PropertyDetails");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "PropertyDetails",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyDetails_ApplicationUserId",
                table: "PropertyDetails",
                newName: "IX_PropertyDetails_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDetails_AspNetUsers_UserId",
                table: "PropertyDetails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
