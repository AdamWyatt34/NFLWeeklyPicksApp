using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFLWeeklyPicksAPI.Migrations
{
    public partial class initialcreatepicks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PickTypes",
                columns: table => new
                {
                    PickTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PickType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickTypes", x => x.PickTypeId);
                });

            migrationBuilder.CreateTable(
                name: "UserPicks",
                columns: table => new
                {
                    UserPicksId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    Week = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPicks", x => x.UserPicksId);
                });

            migrationBuilder.CreateTable(
                name: "UserPickLineItems",
                columns: table => new
                {
                    UserPickLineItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPickId = table.Column<int>(type: "int", nullable: false),
                    PickTypeId = table.Column<int>(type: "int", nullable: false),
                    CompetitionId = table.Column<long>(type: "bigint", nullable: false),
                    PickTeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPickLineItems", x => x.UserPickLineItemId);
                    table.ForeignKey(
                        name: "FK_UserPickLineItems_PickTypes_PickTypeId",
                        column: x => x.PickTypeId,
                        principalTable: "PickTypes",
                        principalColumn: "PickTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPickLineItems_UserPicks_UserPickId",
                        column: x => x.UserPickId,
                        principalTable: "UserPicks",
                        principalColumn: "UserPicksId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_UserPickLineItems_PickTypeId",
                table: "UserPickLineItems",
                column: "PickTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPickLineItems_UserPickId",
                table: "UserPickLineItems",
                column: "UserPickId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPickPoints_UserPickLineItemId",
                table: "UserPickPoints",
                column: "UserPickLineItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPickPoints");

            migrationBuilder.DropTable(
                name: "UserPickLineItems");

            migrationBuilder.DropTable(
                name: "PickTypes");

            migrationBuilder.DropTable(
                name: "UserPicks");
        }
    }
}
