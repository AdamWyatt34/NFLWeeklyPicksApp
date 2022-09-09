using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFLWeeklyPicksAPI.Migrations
{
    public partial class addpropsseasonweeks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeasonWeeks_Seasons_SeasonId",
                table: "SeasonWeeks");

            migrationBuilder.AlterColumn<int>(
                name: "SeasonId",
                table: "SeasonWeeks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonWeeks_Seasons_SeasonId",
                table: "SeasonWeeks",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "SeasonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeasonWeeks_Seasons_SeasonId",
                table: "SeasonWeeks");

            migrationBuilder.AlterColumn<int>(
                name: "SeasonId",
                table: "SeasonWeeks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonWeeks_Seasons_SeasonId",
                table: "SeasonWeeks",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "SeasonId");
        }
    }
}
