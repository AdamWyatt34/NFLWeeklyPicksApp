using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFLWeeklyPicksAPI.Migrations
{
    public partial class updatemodelsforpoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPickPoints");

            migrationBuilder.AddColumn<int>(
                name: "PickPoints",
                table: "UserPickLineItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickPoints",
                table: "UserPickLineItems");

            migrationBuilder.CreateTable(
                name: "UserPickPoints",
                columns: table => new
                {
                    UserPickPointsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPickLineItemId = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPickPoints", x => x.UserPickPointsId);
                    table.ForeignKey(
                        name: "FK_UserPickPoints_UserPickLineItems_UserPickLineItemId",
                        column: x => x.UserPickLineItemId,
                        principalTable: "UserPickLineItems",
                        principalColumn: "UserPickLineItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPickPoints_UserPickLineItemId",
                table: "UserPickPoints",
                column: "UserPickLineItemId",
                unique: true);
        }
    }
}
