using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFLWeeklyPicksAPI.Migrations
{
    public partial class pickpoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserPickPoints_UserPickLineItemId",
                table: "UserPickPoints");

            migrationBuilder.CreateIndex(
                name: "IX_UserPickPoints_UserPickLineItemId",
                table: "UserPickPoints",
                column: "UserPickLineItemId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserPickPoints_UserPickLineItemId",
                table: "UserPickPoints");

            migrationBuilder.CreateIndex(
                name: "IX_UserPickPoints_UserPickLineItemId",
                table: "UserPickPoints",
                column: "UserPickLineItemId");
        }
    }
}
