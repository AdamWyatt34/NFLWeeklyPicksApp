using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFLWeeklyPicksAPI.Migrations
{
    public partial class AddSeasonAndWeeks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    SeasonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.SeasonId);
                });

            migrationBuilder.CreateTable(
                name: "SeasonWeeks",
                columns: table => new
                {
                    SeasonWeeksId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekNumber = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WeekDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeasonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonWeeks", x => x.SeasonWeeksId);
                    table.ForeignKey(
                        name: "FK_SeasonWeeks_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "SeasonId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeasonWeeks_SeasonId",
                table: "SeasonWeeks",
                column: "SeasonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeasonWeeks");

            migrationBuilder.DropTable(
                name: "Seasons");
        }
    }
}
