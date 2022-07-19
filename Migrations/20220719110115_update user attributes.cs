using Microsoft.EntityFrameworkCore.Migrations;

namespace coop2._0.Migrations
{
    public partial class updateuserattributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81a348bb-234d-4971-a243-432cf728b487");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f19d8595-e18d-4460-b79d-b3e787e16911");

            migrationBuilder.RenameColumn(
                name: "CifNumber",
                table: "AspNetUsers",
                newName: "Phone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "AspNetUsers",
                newName: "CifNumber");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f19d8595-e18d-4460-b79d-b3e787e16911", "38d8299b-a845-4e3c-998c-01001837a91a", "ADMIN", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "81a348bb-234d-4971-a243-432cf728b487", "2f9bea6b-1115-4623-8b17-ef59ed1f1cbd", "USER", "USER" });
        }
    }
}
