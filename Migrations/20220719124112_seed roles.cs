using Microsoft.EntityFrameworkCore.Migrations;

namespace coop2._0.Migrations
{
    public partial class seedroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4571156f-dee6-44c8-a142-10e1c7f47404", "442a38e2-fddf-4617-b0e1-63cab048989e", "ADMIN", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "695029a0-64ac-4807-b9df-13ab882667fc", "dc589fba-16e4-4cb4-8222-a2e10044efc2", "USER", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4571156f-dee6-44c8-a142-10e1c7f47404");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "695029a0-64ac-4807-b9df-13ab882667fc");
        }
    }
}
