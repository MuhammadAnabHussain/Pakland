using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pakland.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToPropertyDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PropertyDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDetails_UserId",
                table: "PropertyDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDetails_AspNetUsers_UserId",
                table: "PropertyDetails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDetails_AspNetUsers_UserId",
                table: "PropertyDetails");

            migrationBuilder.DropIndex(
                name: "IX_PropertyDetails_UserId",
                table: "PropertyDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PropertyDetails");
        }
    }
}
