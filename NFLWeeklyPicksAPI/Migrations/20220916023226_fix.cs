using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFLWeeklyPicksAPI.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_SeasonWeeks_SeasonWeeksId",
                table: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Teams_AwayTeamId",
                table: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Teams_HomeTeamId",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_AwayTeamId",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_HomeTeamId",
                table: "Competitions");
            
            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_SeasonWeeks_SeasonWeeksId",
                table: "Competitions",
                column: "SeasonWeeksId",
                principalTable: "SeasonWeeks",
                principalColumn: "SeasonWeeksId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_SeasonWeeks_SeasonWeeksId",
                table: "Competitions");
            
            migrationBuilder.CreateIndex(
                name: "IX_Competitions_AwayTeamId",
                table: "Competitions",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_HomeTeamId",
                table: "Competitions",
                column: "HomeTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_SeasonWeeks_SeasonWeeksId",
                table: "Competitions",
                column: "SeasonWeeksId",
                principalTable: "SeasonWeeks",
                principalColumn: "SeasonWeeksId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Teams_AwayTeamId",
                table: "Competitions",
                column: "AwayTeamId",
                principalTable: "Teams",
                principalColumn: "TeamsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Teams_HomeTeamId",
                table: "Competitions",
                column: "HomeTeamId",
                principalTable: "Teams",
                principalColumn: "TeamsId");
        }
    }
}
