using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFLWeeklyPicksAPI.Migrations
{
    public partial class AddRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "33a8409e-baf3-41a9-81d4-8b87eaafbafa", "1b315656-f14b-46aa-a235-dc93d59687c2", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "917be63d-1c37-42fc-a443-a110e0c795f6", "4a743498-553d-4c1c-976f-107af46a32e7", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33a8409e-baf3-41a9-81d4-8b87eaafbafa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "917be63d-1c37-42fc-a443-a110e0c795f6");
        }
    }
}
