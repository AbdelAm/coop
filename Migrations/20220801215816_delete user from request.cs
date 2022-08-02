using Microsoft.EntityFrameworkCore.Migrations;

namespace coop2._0.Migrations
{
    public partial class deleteuserfromrequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_UserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_UserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Requests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Requests",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UserId",
                table: "Requests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_UserId",
                table: "Requests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
