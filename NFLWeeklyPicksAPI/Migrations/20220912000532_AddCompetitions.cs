using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFLWeeklyPicksAPI.Migrations
{
    public partial class AddCompetitions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "UserPicks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CompetitionsId",
                table: "UserPickLineItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(75)", nullable: true),
                    Nickname = table.Column<string>(type: "nvarchar(75)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    LogoURL = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Abbreviation = table.Column<string>(type: "nvarchar(25)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamsId);
                });

            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    CompetitionsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonWeeksId = table.Column<int>(type: "int", nullable: false),
                    GameName = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    GameDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HomeTeamId = table.Column<int>(type: "int", nullable: false),
                    AwayTeamId = table.Column<int>(type: "int", nullable: false),
                    Odds = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    HomeTeamScoreUrl = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    AwayTeamScoreUrl = table.Column<string>(type: "nvarchar(300)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.CompetitionsId);
                    table.ForeignKey(
                        name: "FK_Competitions_SeasonWeeks_SeasonWeeksId",
                        column: x => x.SeasonWeeksId,
                        principalTable: "SeasonWeeks",
                        principalColumn: "SeasonWeeksId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Competitions_Teams_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamsId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Competitions_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamsId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TeamSeasonRecords",
                columns: table => new
                {
                    TeamSeasonRecordsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    Record = table.Column<string>(type: "nvarchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamSeasonRecords", x => x.TeamSeasonRecordsId);
                    table.ForeignKey(
                        name: "FK_TeamSeasonRecords_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "SeasonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamSeasonRecords_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamsId",
                        onDelete: ReferentialAction.Cascade);
                });
            
            migrationBuilder.CreateIndex(
                name: "IX_UserPickLineItems_CompetitionsId",
                table: "UserPickLineItems",
                column: "CompetitionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_AwayTeamId",
                table: "Competitions",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_HomeTeamId",
                table: "Competitions",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_SeasonWeeksId",
                table: "Competitions",
                column: "SeasonWeeksId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamSeasonRecords_SeasonId",
                table: "TeamSeasonRecords",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamSeasonRecords_TeamId",
                table: "TeamSeasonRecords",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPickLineItems_Competitions_CompetitionsId",
                table: "UserPickLineItems",
                column: "CompetitionsId",
                principalTable: "Competitions",
                principalColumn: "CompetitionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPickLineItems_Competitions_CompetitionsId",
                table: "UserPickLineItems");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "TeamSeasonRecords");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_UserPickLineItems_CompetitionsId",
                table: "UserPickLineItems");
            
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "UserPicks");

            migrationBuilder.DropColumn(
                name: "CompetitionsId",
                table: "UserPickLineItems");
            
        }
    }
}
