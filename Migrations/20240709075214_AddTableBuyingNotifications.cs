using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pakland.Migrations
{
    public partial class AddTableBuyingNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyingNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false), // Adjusted column name
                    IsSeen = table.Column<bool>(type: "bit", nullable: false) // Adjusted column name
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyingNotifications", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyingNotifications");
        }
    }
}
