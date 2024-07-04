using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pakland.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVirtualApplicationUserAttrivuteandForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDetails_AspNetUsers_ApplicationUserId",
                table: "PropertyDetails");

            migrationBuilder.DropIndex(
                name: "IX_PropertyDetails_ApplicationUserId",
                table: "PropertyDetails");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "PropertyDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "PropertyDetails",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDetails_ApplicationUserId",
                table: "PropertyDetails",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDetails_AspNetUsers_ApplicationUserId",
                table: "PropertyDetails",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
