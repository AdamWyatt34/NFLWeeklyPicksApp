using Microsoft.EntityFrameworkCore.Migrations;
using NFLWeeklyPicksAPI.Models.Entities;

#nullable disable

namespace NFLWeeklyPicksAPI.Migrations
{
    public partial class updateenum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PickTypes",
                column: "PickType",
                value: 1
                );

            migrationBuilder.InsertData(
                table: "PickTypes",
                column: "PickType",
                value: 2
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}